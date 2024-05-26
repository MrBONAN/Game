using System.Collections.Generic;
using System.Linq;

namespace MazeMiniGame
{
    public static class WiresStateFormer
    {
        public static Dictionary<char, WireType> translateDict = new Dictionary<char, WireType>()
        {
            { '-', WireType.Default },
            { '_', WireType.Long },
            { '/', WireType.Corner },
            { '|', WireType.LongCorner },
            { '?', WireType.Bridge }
        };
        public static WireField GetLevel(int number)
        {
            switch (number)
            {
                case 1:
                    return MakeWireField1();
            }

            return null;
        }

        private static WireField MakeWireField1()
        {
            var field = new WireField();
            var stringField = new string[]
            {
                "/|?-",
                "_-?/"
            };
            var positions = new string[]
            {
                "(5, 5)|(15, 9)|(25, 40)|(56, 70)",
                "(5, 85)|(15, 29)|(25, 70)|(56, 99)"
            };
            //ToDo scales for game
            field.scaleX = 5;
            field.scaleY = 5;
            field.SetField(stringField, positions);
            return field;
        }
    }
}