using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class SocketBonus : Bonus
    {
        public SocketBonus(int id, string description)
        {
            Id = id;
            Description = description;
        }

        private int _id;
        private string _description;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public override string ToString()
        {
            return Description;
        }

        public static SocketBonus[] GetBonusList()
        {
            return new SocketBonus[]
            {
                new SocketBonus(0, "No Bonus"),
                new SocketBonus(69, "+2 Strength"),
                new SocketBonus(3879, "+3 Strength"),
                new SocketBonus(106, "+4 Strength"),
                new SocketBonus(3357, "+6 Strength"),
                new SocketBonus(3312, "+8 Strength"),
                new SocketBonus(75, "+2 Agility"),
                new SocketBonus(76, "+3 Agility"),
                new SocketBonus(90, "+4 Agility"),
                new SocketBonus(3355, "+6 Agility"),
                new SocketBonus(3313, "+8 Agility"),
                new SocketBonus(2925, "+3 Stamina"),
                new SocketBonus(2883, "+4 Stamina"),
                new SocketBonus(104, "+6 Stamina"),
                new SocketBonus(3307, "+9 Stamina"),
                new SocketBonus(3305, "+12 Stamina"),
                new SocketBonus(3016, "+2 Intellect"),
                new SocketBonus(2863, "+3 Intellect"),
                new SocketBonus(2869, "+4 Intellect"),
                new SocketBonus(3310, "+6 Intellect"),
                new SocketBonus(3353, "+8 Intellect"),
                new SocketBonus(2866, "+3 Spirit"),
                new SocketBonus(2890, "+4 Spirit"),
                new SocketBonus(3311, "+6 Spirit"),
                new SocketBonus(3352, "+8 Spirit"),
                new SocketBonus(2974, "+7 Healing"),
                new SocketBonus(2872, "+9 Healing"),
                new SocketBonus(2881, "+1 Mana per 5 sec"),
                new SocketBonus(2865, "+2 Mana per 5 sec"),
                new SocketBonus(2854, "+3 Mana per 5 sec"),
                new SocketBonus(424, "+2 Spell Power"),
                new SocketBonus(2910, "+3 Spell Power"),
                new SocketBonus(2900, "+4 Spell Power"),
                new SocketBonus(426, "+5 Spell Power"),
                new SocketBonus(3602, "+7 Spell Power"),
                new SocketBonus(3753, "+9 Spell Power"),
                new SocketBonus(1583, "+4 Attack Power"),
                new SocketBonus(1584, "+6 Attack Power"),
                new SocketBonus(1585, "+8 Attack Power"),
                new SocketBonus(3356, "+12 Attack Power"),
                new SocketBonus(3877, "+16 Attack Power"),
                new SocketBonus(2867, "+2 Resillience"),
                new SocketBonus(2862, "+3 Resillience"),
                new SocketBonus(2878, "+4 Resillience"),
                new SocketBonus(3600, "+6 Resillience"),
                new SocketBonus(3821, "+8 Resillience"),
                new SocketBonus(2886, "+2 Hit Rating"),
                new SocketBonus(2860, "+3 Hit Rating"),
                new SocketBonus(2873, "+4 Hit Rating"),
                new SocketBonus(3351, "+6 Hit Rating"),
                new SocketBonus(2909, "+2 Spell Hit Rating"),
                new SocketBonus(2880, "+3 Spell Hit Rating"),
                new SocketBonus(2908, "+4 Spell Hit Rating"),
                new SocketBonus(2887, "+3 Crit Rating"),
                new SocketBonus(2874, "+4 Crit Rating"),
                new SocketBonus(3301, "+6 Crit Rating"),
                new SocketBonus(2787, "+8 Crit Rating"),
                new SocketBonus(2884, "+2 Spell Crit Rating"),
                new SocketBonus(2875, "+3 Spell Crit Rating"),
                new SocketBonus(2864, "+4 Spell Crit Rating"),
                new SocketBonus(2907, "+2 Parry"),
                new SocketBonus(2870, "+3 Parry"),
                new SocketBonus(3359, "+4 Parry"),
                new SocketBonus(3360, "+8 Parry"),
                new SocketBonus(2926, "+2 Dodge"),
                new SocketBonus(2876, "+3 Dodge"),
                new SocketBonus(2871, "+4 Dodge"),
                new SocketBonus(3358, "+6 Dodge"),
                new SocketBonus(2976, "+2 Defense Rating"),
                new SocketBonus(2861, "+3 Defense Rating"),
                new SocketBonus(2932, "+4 Defense Rating"),
                new SocketBonus(3751, "+6 Defense Rating"),
                new SocketBonus(3302, "+8 Defense Rating"),
                new SocketBonus(3017, "+3 Block"),
                new SocketBonus(2972, "+4 Block"),
                new SocketBonus(2888, "+6 Block"),
                new SocketBonus(3363, "+9 Block"),
                new SocketBonus(3094, "+4 Expertise"),
                new SocketBonus(3362, "+6 Expertise"),
                new SocketBonus(3778, "+8 Expertise"),
                new SocketBonus(3267, "+4 Haste Rating"),
                new SocketBonus(3309, "+6 Haste Rating"),
                new SocketBonus(3303, "+8 Haste Rating"),
            };
        }
       
    }
}
