using System;
using System.Collections.Generic;
namespace csm
{
    public enum ColourSpace { RGB, HSL, HSV };

    public class Colour
    {
        public const float Opaque = 1f;
        public const float Transparent = 1f;
        ColourSpace space;
        float[] values;
        float alpha;
        public Colour(float [] values, float alpha, ColourSpace space = ColourSpace.RGB)
        {
            this.values = values;
            this.space = space;
            this.alpha = alpha;
        }
        public Colour(float firstVal, float secondVal, float thirdVal, float alpha, ColourSpace space = ColourSpace.RGB)
        {
            values = new float[3] { firstVal, secondVal, thirdVal };
            this.space = space;
            this.alpha = alpha;
        }

        public float[] getValues()
        {
            return values;
        }
        public float[] getValues(ColourSpace space)
        {
            if (this.space == space)
                return values;
            else
                return ConvertTo(values, this.space, space);
        }
        public void setColourSpace(ColourSpace newSpace)
        {
            if (space == newSpace)
                return;
            values = ConvertTo(values, space, newSpace);
            space = newSpace;
            
        }

        public ColourSpace getColourSpace() { return space; }

        public static float[] ToHSL(float[] values, ColourSpace space) {
            float h, s, l;
            switch (space)
            {
                case ColourSpace.RGB:
                    float max = System.Math.Max(values[0], System.Math.Max(values[1], values[2]));
                    float min = System.Math.Min(values[0], System.Math.Min(values[1], values[2]));
                    if (max == min)
                        h = 0;
                    else if (max == values[0])
                        h = 60 * ((values[1] - values[2]) / (max - min));
                    else if (max == values[1])
                        h = 60 * (2+(values[2] - values[0]) / (max - min));
                    else
                        h = 60 * (4 + (values[0] - values[1]) / (max - min));

                        s =(max == 0 || min == 1)? 0:(max - min) / (1 - System.Math.Abs(max - min - 1));

                        l = (max + min / 2);
                    break;
                case ColourSpace.HSV:
                    l = values[2] * (1 - values[1] / 2);
                    h = values[0];
                    s = (l == 0 || l == 1) ? 0 : (values[2] - l) / System.Math.Min(l, 1 - l);
                    break;
                default:
                    return values;
            }
            return new float[3] { h, s, l };
        }
        public static float[] ToRGB(float[] values, ColourSpace space)
        {
            float r, g, b;
            switch (space)
            {
                case ColourSpace.HSV:
                    float h, f, p, q, t;
                    h = (float)System.Math.Floor(values[0] / 60f);
                    f = values[2] / 60 - h;
                    p = values[2] * (1 - values[1]);
                    q = values[2] * (1 - values[1] * f);
                    t = values[2] * (1 - values[1] * (1 - f));
                    switch (h)
                    {
                        case 1:
                            r = q;
                            g = values[2];
                            b = p;
                            break;
                        case 2:
                            r = p;
                            g = values[2];
                            b = t;
                            break;
                        case 3:
                            r = p;
                            g = q;
                            b = values[2];
                            break;
                        case 4:
                            r = t;
                            g = p;
                            b = values[2];
                            break;
                        case 5:
                            r = values[2];
                            g = p;
                            b = q;
                            break;
                        default:
                            r = values[2];
                            g = t;
                            b = p;
                            break;
                    }
                    break;
                case ColourSpace.HSL:
                    r = values[2] - (values[1] * System.Math.Min(values[2], 1 - values[2])) * System.Math.Max(-1, System.Math.Min(System.Math.Min((0 + values[0] / 30f) % 12 - 3, 9 - (0 + values[0] / 30) % 12), 1));
                    g = values[2] - (values[1] * System.Math.Min(values[2], 1 - values[2])) * System.Math.Max(-1, System.Math.Min(System.Math.Min((8 + values[0] / 30f) % 12 - 3, 9 - (8 + values[0] / 30) % 12), 1));
                    b = values[2] - (values[1] * System.Math.Min(values[2], 1 - values[2])) * System.Math.Max(-1, System.Math.Min(System.Math.Min((4 + values[0] / 30f) % 12 - 3, 9 - (4 + values[0] / 30) % 12), 1));
                    break;
                default:
                    return values;
            }
            return new float[] { r, g, b };
        }
        public static float[] ToHSV(float[] values, ColourSpace space)
        {
            float h, s, v;

            switch (space)
            {
                case ColourSpace.RGB:
                    float max, min;
                    max = System.Math.Max(values[0], System.Math.Max(values[1], values[2]));
                    min = System.Math.Min(values[0], System.Math.Min(values[1], values[2]));
                    if (max == min)
                        h = 0;
                    else if (max == values[0])
                        h = 60 * ((values[1] - values[2]) / (max - min));
                    else if (max == values[1])
                        h = 60 * (2 + (values[2] - values[0]) / (max - min));
                    else
                        h = 60 * (4 + (values[0] - values[1]) / (max - min));

                    s = (max == 0) ? 0:(max-min)/max;
                    v = max;
                    break;
                case ColourSpace.HSL:
                    v = values[2] + values[1] * System.Math.Min(values[2], 1 - values[2]);
                    h = values[0];
                    s = (v == 0) ? 0:2*(1-values[2]/v);
                    break;
                default:
                    return values;
            }
                

            return new float [] { h,s,v};
        }
        public static Colour ConvertTo (Colour colour, ColourSpace desiredSpace)
        {
            switch(desiredSpace)
            {
                case ColourSpace.HSL:
                    return new Colour(ToHSL(colour.getValues(), colour.space), colour.alpha, desiredSpace);
                case ColourSpace.HSV:
                    return new Colour(ToHSV(colour.getValues(), colour.space), colour.alpha, desiredSpace);
                default:
                    return new Colour(ToRGB(colour.getValues(), colour.space), colour.alpha, desiredSpace);
            }
        }
        public static float[] ConvertTo(float[] values, ColourSpace currentSpace, ColourSpace desiredSpace)
        {
            switch (desiredSpace)
            {
                case ColourSpace.HSL:
                    return ToHSL(values, currentSpace);
                case ColourSpace.HSV:
                    return ToHSV(values, currentSpace);
                default:
                    return ToRGB(values, currentSpace);
            }
        }
}



    public class ColourScheme
    {
        private Dictionary<string, Colour> colourList;
        public ColourScheme(string[] keys, Colour[] values)
    {
        colourList = new Dictionary<string, Colour>();
        for (int i = 0; i < keys.Length && i < values.Length; i++)
        {
                colourList.Add(keys[i], values[i]);
        }
    }
        public ColourScheme(Dictionary<string, Colour> dict)
        {
            colourList = dict;
        }
        public ColourScheme()
        {
            colourList = new Dictionary<string, Colour>();
        }

        public Colour getColour(string key)
        {
            Colour returnValue = null;
            try
            {
                returnValue =  colourList[key];
            }
            catch (KeyNotFoundException)
            {

            }
            return returnValue;
        }

        public bool addPair(string key, Colour value)
        {
            try
            {
                colourList.Add(key, value);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
        public bool changeColour(string key, Colour newValue)
        {
            try
            {
                colourList[key] = newValue;
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }


    public class CSMManager
    {
        
    }
}
   
