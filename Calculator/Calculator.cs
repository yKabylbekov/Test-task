using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator
{
    public class Calculator
    {
        private static readonly Dictionary<char, int> _roman_numbers = new Dictionary<char, int>
        {
            {'I', 1},
            {'V', 5},
            {'X', 10},
            {'L', 50},
            {'C', 100},
            {'D', 500},
            {'M', 1000}
        };

        public static string Start( string input )
        {
            string result = string.Empty;
            Dictionary<int, char> brakets = new Dictionary<int, char>();
            for( int i = 0; i < input.Length; i++ ) {
                if( input[ i ] == '(' ) {
                    brakets.Add( i, '(' );
                }
                else if( input[ i ] == ')' ) {
                    brakets.Add( i, ')' );
                }
            }
            if( brakets.Count > 2 ) {
                for( int i = 0; i < brakets.Count; i += 2 ) {
                    int first_braket = input.IndexOf( brakets[ brakets.Keys.ToList()[ i ] ] );
                    int second_braket = input.IndexOf( brakets[ brakets.Keys.ToList()[ i + 1 ] ] );
                    string nums = input.Substring( first_braket + 1, second_braket - first_braket - 1 ).Trim();
                    string braket_result = Calculate( nums );
                    input = input.Replace( $"({nums})", $"{braket_result}" ).Trim();
                }
                result = Calculate( input );
            }
            else {
                for( int i = 0; i < brakets.Count; i += 2 ) {
                    int first_braket = input.IndexOf( brakets[ brakets.Keys.ToList()[ i ] ] );
                    int second_braket = input.IndexOf( brakets[ brakets.Keys.ToList()[ i + 1 ] ] );
                    if( first_braket != -1 && second_braket == -1 || first_braket == -1 && second_braket != -1 )
                        throw new Exception( "There is only one braket" );
                    if( first_braket == -1 || second_braket == -1 ) {
                        result = Calculate( input );
                    }
                    else {
                        string nums = input.Substring( first_braket + 1, second_braket - first_braket - 1 ).Trim();
                        string braket_result = Calculate( nums );
                        string other_nums = input.Replace( $"({nums})", $"{braket_result}" ).Trim();
                        result = Calculate( other_nums );
                    }
                }
            }

            return result;
        }

        private static string Calculate( string input )
        {
            List<string> splited_input = new List<string>();
            string number = string.Empty;
            List<string> signs = new List<string>();
            bool is_all_multiply = true;
            for( int i = 0; i < input.Length; i++ ) {
                switch( input[ i ] ) {
                    case '*':
                        splited_input.Add( number );
                        number = string.Empty;
                        splited_input.Add( input[ i ].ToString() );
                        signs.Add( input[ i ].ToString() );
                        break;
                    case '+':
                        is_all_multiply = false;
                        splited_input.Add( number );
                        number = string.Empty;
                        splited_input.Add( input[ i ].ToString() );
                        signs.Add( input[ i ].ToString() );
                        break;
                    case '-':
                        is_all_multiply = false;
                        splited_input.Add( number );
                        number = string.Empty;
                        splited_input.Add( input[ i ].ToString() );
                        signs.Add( input[ i ].ToString() );
                        break;
                    case ' ':
                        break;
                    default:
                        number += input[ i ];
                        break;
                }
            }
            splited_input.Add( number );
            int result = 0;
            int count = 0;
            if( is_all_multiply ) {
                for( int i = 0; i < splited_input.Count; i++ ) {
                    if( result == 0 ) {
                        if( splited_input[ i ] == "*" ) {
                            result = ParseRomanNumeral( splited_input[ i - 1 ] ) * ParseRomanNumeral( splited_input[ i + 1 ] );
                        }
                    }
                    else {
                        if( splited_input[ i ] == "*" ) {
                            result *= ParseRomanNumeral( splited_input[ i + 1 ] );
                        }
                    }
                }
            }
            else {
                while( count != signs.Count ) {
                    for( int i = 0; i < splited_input.Count; i++ ) {
                        if( result == 0 ) {
                            if( signs.Contains( "*" ) ) {
                                if( splited_input[ i ] == "*" ) {
                                    result = ParseRomanNumeral( splited_input[ i - 1 ] ) * ParseRomanNumeral( splited_input[ i + 1 ] );
                                    count++;
                                    splited_input[ i ] = "";
                                    splited_input[ i - 1 ] = "";
                                    splited_input[ i + 1 ] = "";
                                }
                            }
                            else {
                                if( splited_input[ i ] == "+" ) {
                                    result = ParseRomanNumeral( splited_input[ i - 1 ] ) + ParseRomanNumeral( splited_input[ i + 1 ] );
                                    count++;
                                    splited_input[ i ] = "";
                                    splited_input[ i - 1 ] = "";
                                    splited_input[ i + 1 ] = "";
                                }
                                else if( splited_input[ i ] == "-" ) {
                                    result = ParseRomanNumeral( splited_input[ i - 1 ] ) - ParseRomanNumeral( splited_input[ i + 1 ] );
                                    count++;
                                    splited_input[ i ] = "";
                                    splited_input[ i - 1 ] = "";
                                    splited_input[ i + 1 ] = "";
                                }
                            }
                        }
                        else {
                            if( splited_input[ i ] == "*" ) {
                                if( splited_input[ i + 1 ] != "" ) {
                                    result *= ParseRomanNumeral( splited_input[ i + 1 ] );
                                    splited_input[ i ] = "";
                                    splited_input[ i + 1 ] = "";
                                }
                                else {
                                    result *= ParseRomanNumeral( splited_input[ i - 1 ] );
                                    splited_input[ i ] = "";
                                    splited_input[ i - 1 ] = "";
                                }
                                count++;
                            }
                            else if( splited_input[ i ] == "+" ) {
                                if( splited_input[ i + 1 ] != "" ) {
                                    result += ParseRomanNumeral( splited_input[ i + 1 ] );
                                    splited_input[ i ] = "";
                                    splited_input[ i + 1 ] = "";
                                }
                                else {
                                    result += ParseRomanNumeral( splited_input[ i - 1 ] );
                                    splited_input[ i ] = "";
                                    splited_input[ i - 1 ] = "";
                                }
                                count++;
                            }
                            else if( splited_input[ i ] == "-" ) {
                                if( splited_input[ i + 1 ] != "" ) {
                                    result -= ParseRomanNumeral( splited_input[ i + 1 ] );
                                    splited_input[ i ] = "";
                                    splited_input[ i + 1 ] = "";
                                }
                                else {
                                    result -= ParseRomanNumeral( splited_input[ i - 1 ] );
                                    splited_input[ i ] = "";
                                    splited_input[ i - 1 ] = "";
                                }
                                count++;
                            }
                        }
                    }
                }
            }

            return ToRomanNumeral( result );
        }

        private static int ParseRomanNumeral( string roman_number )
        {
            int number = 0;
            for( int i = 0; i < roman_number.Length; i++ ) {
                number += _roman_numbers[ roman_number[ i ] ];
                if( i > 0 && _roman_numbers[ roman_number[ i ] ] > _roman_numbers[ roman_number[ i - 1 ] ] ) {
                    number -= _roman_numbers[ roman_number[ i - 1 ] ] * 2;
                }
            }
            return number;
        }

        private static string ToRomanNumeral( int result )
        {
            if( result > 3999 || result < 0 )
                throw new ArgumentOutOfRangeException( "Value must be between 0 and 3999" );

            var roman_result = new List<string>();

            while( result >= 1000 ) {
                roman_result.Add( "M" );
                result -= 1000;
            }

            if( result >= 900 ) {
                roman_result.Add( "CM" );
                result -= 900;
            }

            while( result >= 500 ) {
                roman_result.Add( "D" );
                result -= 500;
            }

            if( result >= 400 ) {
                roman_result.Add( "CD" );
                result -= 400;
            }

            while( result >= 100 ) {
                roman_result.Add( "C" );
                result -= 100;
            }

            if( result >= 90 ) {
                roman_result.Add( "XC" );
                result -= 90;
            }

            while( result >= 50 ) {
                roman_result.Add( "L" );
                result -= 50;
            }

            if( result >= 40 ) {
                roman_result.Add( "XL" );
                result -= 40;
            }

            while( result >= 10 ) {
                roman_result.Add( "X" );
                result -= 10;
            }

            if( result >= 9 ) {
                roman_result.Add( "IX" );
                result -= 9;
            }

            while( result >= 5 ) {
                roman_result.Add( "V" );
                result -= 5;
            }

            if( result >= 4 ) {
                roman_result.Add( "IV" );
                result -= 4;
            }

            while( result >= 1 ) {
                roman_result.Add( "I" );
                result -= 1;
            }

            return string.Join( "", roman_result );
        }
    }
}
