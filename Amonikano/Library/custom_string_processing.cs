#define COMPLETE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amonikano.Library
{
#if COMPLETE || DEBUG
    //TODO: Async support
    public class custom_string_processor
    {
        public enum logical_group
        {
            and = 0,
            or = 1,
        }
        public enum lookup_type
        {
            prefix = 0,
            suffix = 1,
            anywhere = 2,
        }
        Dictionary<lookup_type, char[]> char_look_up;
        Dictionary<lookup_type, string[]> str_look_up;
        bool cut_all_spaces = true;
        const logical_group default_logic = logical_group.or;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="desired_char_dict"> Please use char for a bit boost of performance if possible</param>
        /// <param name="desired_string_dict"></param>
        /// <param name="cut_off_spaces">default: true. Although it might have something to the performance, but it should be neccessary yeah</param>
        public custom_string_processor(Dictionary<lookup_type, char[]> desired_char_dict,
            Dictionary<lookup_type, string[]> desired_string_dict, bool cut_off_spaces)
        {
            this.char_look_up = desired_char_dict;
            this.str_look_up = desired_string_dict;
            this.cut_all_spaces = cut_off_spaces;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="desired_char_dict"> Please use char for a bit boost of performance if possible</param>
        /// <param name="desired_string_dict"></param>
        public custom_string_processor(Dictionary<lookup_type, char[]> desired_char_dict,
            Dictionary<lookup_type, string[]> desired_string_dict)
        {
            this.char_look_up = desired_char_dict;
            this.str_look_up = desired_string_dict;
        }
        public custom_string_processor(lookup_type lookup_type, params char[] all_chars)
        {
            char_look_up = new Dictionary<lookup_type, char[]>();
        }

        public custom_string_processor(lookup_type lookup_type, params string[] all_str)
        {
            str_look_up = new Dictionary<lookup_type, string[]>();
        }
        [System.Flags]
        enum matches
        {
            none = 0,
            prefix_char = 1,
            prefix_str = 2,
            suffix_char = 4,
            suffix_str = 8,

            all = prefix_char | prefix_str | suffix_char | suffix_str,
        }
        //TODO: add throwing errors out
        public bool string_satisfied(string input)
        {
#if DEBUG
            //Throw out the values of the lookup tables
            if (char_look_up != null)
            {
                string char_lookup_stringout = "";
                foreach (KeyValuePair<lookup_type, char[]> pair in char_look_up)
                {
                    char_lookup_stringout += "{ " + pair.Key + ", " + new string(pair.Value) + " },\n";
                }
                Console.WriteLine("char_lookup: " + char_lookup_stringout);
            }
            else
            {
                Console.WriteLine("char_lookup is null");
            }
            if (str_look_up != null)
            {
                string str_lookup_stringout = "";
                foreach (KeyValuePair<lookup_type, string[]> pair in str_look_up)
                {
                    str_lookup_stringout += "{ " + pair.Key + ", " + string.Join(", ", pair.Value) + " },\n";
                }
                Console.WriteLine("str_lookup: " + str_lookup_stringout);
            }
            else
            {
                Console.WriteLine("str_lookup is null");
            }
#endif
            bool result = true;
#if DEBUG
            Console.WriteLine("Input is {0}", input);
#endif 
            //cutoff spaces
            if (cut_all_spaces)
            {
                input = input.Trim();
            }
            string[] elements = input.Split();
            matches match = matches.none;
#if DEBUG
            string elementz = string.Join(", ", elements);
            Console.WriteLine("Input (trimmed) is: {0}\nElements: {1}", input, elementz);
#endif
            if (char_lookup(ref result, elements, lookup_type.prefix))
            {
#if DEBUG
                Console.WriteLine("Passed char prefix");
#endif
                match |= matches.prefix_char;
            }
            if (char_lookup(ref result, elements, lookup_type.suffix))
            {
#if DEBUG
                Console.WriteLine("Passed char suffix");
#endif
                match |= matches.suffix_char;
            }
            if (str_lookup(ref result, elements, lookup_type.prefix))
            {
#if DEBUG
                Console.WriteLine("Passed str prefix");
#endif
                match |= matches.prefix_str;
            }
            if (str_lookup(ref result, elements, lookup_type.suffix))
            {
#if DEBUG
                Console.WriteLine("Passed str suffix");
#endif
                match |= matches.suffix_str;
            }
            if (match != matches.all)
            {
                Console.WriteLine("Not a command");
                return false;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="elements"></param>
        /// <param name="type"></param>
        /// <returns>true if nothing to compare to, true if at least one match</returns>
        private bool char_lookup(ref bool result, string[] elements, lookup_type type)
        {
            if (char_look_up == null)
                return true;
            if (char_look_up.ContainsKey(type))
            {
                result = false;
                char fix;
                switch (type)
                {
                    case lookup_type.prefix:
                        fix = elements[0][0];
                    break;

                    case lookup_type.suffix:
                        string last = elements[elements.Length - 1];
                        fix = last[last.Length - 1];
                    break;
                    default:
                    return false;
                }
                int i = 0;
                char[] all_comparisons = char_look_up[type];
                int count = all_comparisons.Length;
#if DEBUG
                Console.WriteLine("char_lookup is going in a loop");
#endif
                for (; i < count; i++)
                {
                    if (all_comparisons[i] == fix)
                    {
                        result = true;
                        //matches on prefix char
                        break;
                    }
                }
#if DEBUG
                Console.WriteLine("char_lookup is out of a loop");
#endif
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="elements"></param>
        /// <param name="type"></param>
        /// <returns>true if nothing to compare to, true if at least one match</returns>
        private bool str_lookup(ref bool result, string[] elements, lookup_type type)
        {
            if (str_look_up == null)
                return true;
            if (str_look_up.ContainsKey(type))
            {
                result = false;
                string prefix = elements[0];
                //if char count of prefix less than min of lookup table, we can prolly skip the search
                int i = 0;
                string[] all_comparisons = str_look_up[type];
                int count = all_comparisons.Length;
                if (type == lookup_type.prefix)
                {
#if DEBUG
                    Console.WriteLine("str_lookup is going in a loop");
#endif
                    for (; i < count; i++)
                    {
                        if (prefix.StartsWith(all_comparisons[i]))
                        {
                            result = true;
                            break;
                        }
                    }
#if DEBUG
                    Console.WriteLine("str_lookup is out of a loop");
#endif
                }
                else if (type == lookup_type.suffix)
                {
#if DEBUG
                    Console.WriteLine("str_lookup is going in a loop");
#endif
                    for (; i < count; i++)
                    {
                        if (prefix.EndsWith(all_comparisons[i]))
                        {
                            result = true;
                            break;
                        }
                    }
#if DEBUG
                    Console.WriteLine("str_lookup is out of a loop");
#endif
                }

            }
            return result;
        }

        public static string TrimAllWithInplaceCharArray(string str)
        {

            int len = str.Length;
            char[] src = str.ToCharArray();
            int dstIdx = 0;

            for (int i = 0; i < len; i++)
            {
                char ch = src[i];

                switch (ch)
                {

                    case '\u0020':
                    case '\u00A0':
                    case '\u1680':
                    case '\u2000':
                    case '\u2001':

                    case '\u2002':
                    case '\u2003':
                    case '\u2004':
                    case '\u2005':
                    case '\u2006':

                    case '\u2007':
                    case '\u2008':
                    case '\u2009':
                    case '\u200A':
                    case '\u202F':

                    case '\u205F':
                    case '\u3000':
                    case '\u2028':
                    case '\u2029':
                    case '\u0009':

                    case '\u000A':
                    case '\u000B':
                    case '\u000C':
                    case '\u000D':
                    case '\u0085':
                    continue;

                    default:
                    src[dstIdx++] = ch;
                    break;
                }
            }
            return new string(src, 0, dstIdx);
        }

    }
#endif
}
