using System;
using System.Collections.Generic;
using System.IO;

namespace DBCViewer
{
    public class AreaTableRecord
    {
        public int ID { get; private set; }
        public int ContinentID { get; private set; }
        public int ParentAreaID { get; private set; }
        public int AreaBit { get; private set; }
        public int flags_0 { get; private set; }
        public int flags_1 { get; private set; }
        public int SoundProviderPref { get; private set; }
        public int SoundProviderPrefUnderwater { get; private set; }
        public int AmbienceID { get; private set; }
        public int ZoneMusic { get; private set; }
        public string ZoneName { get; private set; }
        public int IntroSound { get; private set; }
        public int ExplorationLevel { get; private set; }
        public string AreaName_lang { get; private set; }
        public int factionGroupMask { get; private set; }
        public int liquidTypeID_0 { get; private set; }
        public int liquidTypeID_1 { get; private set; }
        public int liquidTypeID_2 { get; private set; }
        public int liquidTypeID_3 { get; private set; }
        public float ambient_multiplier { get; private set; }
        public int mountFlags { get; private set; }
        public int uwIntroSound { get; private set; }
        public int uwZoneMusic { get; private set; }
        public int uwAmbience { get; private set; }
        public int world_pvp_id { get; private set; }
        public int pvpCombatWorldStateID { get; private set; }
        public int wildBattlePetLevelMin { get; private set; }
        public int wildBattlePetLevelMax { get; private set; }
        public int windSettingsID { get; private set; }
    }

    class DBCReaderGeneric<T> where T : new()
    {
        private const uint HeaderSize = 20;
        private const uint DBCFmtSig = 0x43424457;          // WDBC

        public int RecordsCount { get; private set; }
        public int FieldsCount { get; private set; }
        public int RecordSize { get; private set; }
        public int StringTableSize { get; private set; }

        private T[] m_rows;

        public T this[int row]
        {
            get { return m_rows[row]; }
        }

        public DBCReaderGeneric(string fileName)
        {
            using (var reader = BinaryReaderExtensions.FromFile(fileName))
            {
                if (reader.BaseStream.Length < HeaderSize)
                {
                    throw new InvalidDataException(String.Format("File {0} is corrupted!", fileName));
                }

                if (reader.ReadUInt32() != DBCFmtSig)
                {
                    throw new InvalidDataException(String.Format("File {0} isn't valid DBC file!", fileName));
                }

                RecordsCount = reader.ReadInt32();
                FieldsCount = reader.ReadInt32();
                RecordSize = reader.ReadInt32();
                StringTableSize = reader.ReadInt32();

                long pos = reader.BaseStream.Position;
                long stringTableStart = reader.BaseStream.Position + RecordsCount * RecordSize;
                reader.BaseStream.Position = stringTableStart;

                Dictionary<int, string> StringTable = new Dictionary<int, string>();

                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    int index = (int)(reader.BaseStream.Position - stringTableStart);
                    StringTable[index] = reader.ReadStringNull();
                }

                reader.BaseStream.Position = pos;

                m_rows = new T[RecordsCount];

                var props = typeof(T).GetProperties();

                for (int i = 0; i < RecordsCount; i++)
                {
                    T row = new T();

                    long rowStart = reader.BaseStream.Position;

                    for (int j = 0; j < props.Length; j++)
                    {
                        var prop = props[j];

                        switch (Type.GetTypeCode(prop.PropertyType))
                        {
                            case TypeCode.Int32:
                                prop.SetValue(row, reader.ReadInt32());
                                break;
                            case TypeCode.UInt32:
                                prop.SetValue(row, reader.ReadUInt32());
                                break;
                            case TypeCode.Single:
                                prop.SetValue(row, reader.ReadSingle());
                                break;
                            case TypeCode.String:
                                prop.SetValue(row, StringTable[reader.ReadInt32()]);
                                break;
                            default:
                                throw new Exception("Unsupported field type " + Type.GetTypeCode(prop.PropertyType));
                        }
                    }

                    if (reader.BaseStream.Position - rowStart != RecordSize)
                    {
                        // struct bigger than record size
                        if (reader.BaseStream.Position - rowStart > RecordSize)
                            throw new Exception("Incorrect DBC struct!");

                        // struct smaller than record size (incomplete)
                        reader.BaseStream.Position += RecordSize - (reader.BaseStream.Position - rowStart);
                    }

                    m_rows[i] = row;
                }
            }
        }
    }
}
