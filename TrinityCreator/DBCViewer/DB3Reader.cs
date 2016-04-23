using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DBCViewer
{
    class DB3Reader : IWowClientDBReader
    {
        private const int HeaderSize = 48;
        private const uint DB3FmtSig = 0x33424457;          // WDB3

        public int RecordsCount { get; private set; }
        public int FieldsCount { get; private set; }
        public int RecordSize { get; private set; }
        public int StringTableSize { get; private set; }

        public Dictionary<int, string> StringTable { get; private set; }

        private SortedDictionary<int, byte[]> Lookup = new SortedDictionary<int, byte[]>();

        public IEnumerable<BinaryReader> Rows
        {
            get
            {
                foreach(var row in Lookup)
                {
                    yield return new BinaryReader(new MemoryStream(row.Value), Encoding.UTF8);
                }
            }
        }

        public DB3Reader(string fileName)
        {
            using (var reader = BinaryReaderExtensions.FromFile(fileName))
            {
                if (reader.BaseStream.Length < HeaderSize)
                {
                    throw new InvalidDataException(String.Format("File {0} is corrupted!", fileName));
                }

                if (reader.ReadUInt32() != DB3FmtSig)
                {
                    throw new InvalidDataException(String.Format("File {0} isn't valid DB2 file!", fileName));
                }

                RecordsCount = reader.ReadInt32();
                FieldsCount = reader.ReadInt32();
                RecordSize = reader.ReadInt32();
                StringTableSize = reader.ReadInt32();

                uint tableHash = reader.ReadUInt32();
                uint build = reader.ReadUInt32();
                uint unk1 = reader.ReadUInt32();

                int MinId = reader.ReadInt32();
                int MaxId = reader.ReadInt32();
                int locale = reader.ReadInt32();
                int CopyTableSize = reader.ReadInt32();

                int stringTableStart = HeaderSize + RecordsCount * RecordSize;
                int stringTableEnd = stringTableStart + StringTableSize;

                // Index table
                int[] m_indexes = null;
                bool hasIndex = stringTableEnd + CopyTableSize < reader.BaseStream.Length;

                if (hasIndex)
                {
                    reader.BaseStream.Position = stringTableEnd;

                    m_indexes = new int[RecordsCount];

                    for (int i = 0; i < RecordsCount; i++)
                        m_indexes[i] = reader.ReadInt32();
                }

                // Records table
                reader.BaseStream.Position = HeaderSize;

                for (int i = 0; i < RecordsCount; i++)
                {
                    byte[] recordBytes = reader.ReadBytes(RecordSize);

                    if (hasIndex)
                    {
                        byte[] newRecordBytes = new byte[RecordSize + 4];

                        Array.Copy(BitConverter.GetBytes(m_indexes[i]), newRecordBytes, 4);
                        Array.Copy(recordBytes, 0, newRecordBytes, 4, recordBytes.Length);

                        Lookup.Add(m_indexes[i], newRecordBytes);
                    }
                    else
                    {
                        Lookup.Add(BitConverter.ToInt32(recordBytes, 0), recordBytes);
                    }
                }

                // Strings table
                reader.BaseStream.Position = stringTableStart;

                StringTable = new Dictionary<int, string>();

                while (reader.BaseStream.Position != stringTableEnd)
                {
                    int index = (int)reader.BaseStream.Position - stringTableStart;
                    StringTable[index] = reader.ReadStringNull();
                }

                // Copy index table
                long copyTablePos = stringTableEnd + (hasIndex ? 4 * RecordsCount : 0);

                if (copyTablePos != reader.BaseStream.Length && CopyTableSize != 0)
                {
                    reader.BaseStream.Position = copyTablePos;

                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        int id = reader.ReadInt32();
                        int idcopy = reader.ReadInt32();

                        RecordsCount++;

                        byte[] copyRow = Lookup[idcopy];
                        byte[] newRow = new byte[copyRow.Length];
                        Array.Copy(copyRow, newRow, newRow.Length);
                        Array.Copy(BitConverter.GetBytes(id), newRow, 4);

                        Lookup.Add(id, newRow);
                    }
                }
            }
        }
    }
}
