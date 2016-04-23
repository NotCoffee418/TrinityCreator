using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DBCViewer
{
    struct MPQHeader	// sizeof 0x10
    {
        public uint mpqMagicNumber;	// MPQ file magic number: 0xDEADBEEF
        public uint fileTypeId;	// file type or version id (same for all *.gam files)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] unused;	// always 0x00000000
    }

    struct StlHeader	// sizeof 0x28
    {
        public uint stlFileId;	// Stl file Id
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] unknown1;	// always 0x00000000
        public uint headerSize;	// size (in bytes) of the StlHeader? (always 0x00000028)
        public int entriesSize;	// size (in bytes) of the StlEntries
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] unknown2;	// always 0x00000000
    }

    struct StlEntry	// sizeof 0x50
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] unknown1;	// always 0x00000000
        public uint string1offset;	// file offset for string1 (non-NLS key)
        public uint string1size;	// size of string1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] unknown2;	// always 0x00000000
        public uint string2offset;	// file offset for string2
        public uint string2size;	// size of string2
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] unknown3;	// always 0x00000000
        public uint string3offset;	// file offset for string3
        public uint string3size;	// size of string3
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] unknown4;	// always 0x00000000
        public uint string4offset;	// file offset for string4
        public uint string4size;	// size of string4
        public uint unknown5;	// always 0xFFFFFFFF
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public uint[] unknown6;	// always 0x00000000
    }

    class STLReader : IWowClientDBReader
    {
        public int RecordsCount { get; private set; }
        public int FieldsCount { get; private set; }
        public int RecordSize { get; private set; }
        public int StringTableSize { get; private set; }

        public Dictionary<int, string> StringTable { get; private set; }

        private byte[][] m_rows;

        public IEnumerable<BinaryReader> Rows
        {
            get
            {
                for (int i = 0; i < RecordsCount; ++i)
                {
                    yield return new BinaryReader(new MemoryStream(m_rows[i]), Encoding.UTF8);
                }
            }
        }

        private BinaryReader reader;

        public STLReader(string fileName)
        {
            reader = BinaryReaderExtensions.FromFile(fileName);

            MPQHeader mHdr = reader.ReadStruct<MPQHeader>();
            StlHeader sHdr = reader.ReadStruct<StlHeader>();
            //StlEntry sEntry = reader.ReadStruct<StlEntry>();

            RecordsCount = sHdr.entriesSize / 0x50;

            m_rows = new byte[RecordsCount][];

            for (int i = 0; i < RecordsCount; ++i)
                m_rows[i] = reader.ReadBytes(0x50);

            //StringTable = new Dictionary<int, string>();

            //if (reader.BaseStream.Position != reader.BaseStream.Length)
            //{
            //    while (reader.BaseStream.Position != reader.BaseStream.Length)
            //    {
            //        if (reader.PeekChar() == 0)
            //        {
            //            reader.BaseStream.Position++;
            //            continue;
            //        }

            //        int offset = (int)reader.BaseStream.Position;
            //        StringTable[offset] = reader.ReadStringNull();
            //    }
            //}
        }

        public string ReadString(int offset)
        {
            reader.BaseStream.Position = offset;

            while(reader.PeekChar() == 0)
                reader.BaseStream.Position++;

            return reader.ReadStringNull();
            //    while (reader.BaseStream.Position != reader.BaseStream.Length)
            //    {
            //        if (reader.PeekChar() == 0)
            //        {
            //            reader.BaseStream.Position++;
            //            continue;
            //        }

            //        int offset = (int)reader.BaseStream.Position;
            //        StringTable[offset] = reader.ReadStringNull();
            //    }
        }
    }
}
