using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Web;
using Microsoft.SqlServer.Server;

namespace DevChallenge
{
    public class Avengers
    {
        private List<long> fibo;
        private char[] validVowels = {'a', 'e', 'i', 'o', 'u', 'y', 'A', 'E', 'I', 'O', 'U', 'Y'};

        private string[] englishWords =
        {
            "drool", "cats", "clean", "code", "dogs", "materials", "needed", "this", "is", "hard",
            "what", "are", "you", "smoking", "shot", "gun", "down", "river", "super", "man", "rule", "acklen",
            "developers", "amazing", "home", "notch", "light", "saw"
        };

        private string[] testArray = {"hEllo", "bOok", "read", "NeEd", "paliNdromE", "happy", "WhAtArEyOuSmOkInG"};
        string[] testArray2 = {"lightsaw","WhAtArEyOuSmOkInG","haaha"};

        public Avengers()
        {
            fibo = FibonacciList(90);
        }


        public string IronMan(string[] input)
        {
            var result = "";
            // step #1
            Array.Sort(input);
            // step #2
            string[] newWords = GetMovedVowelsArray(input);
            // step #3
            result = ConcatenateAscci(newWords);
            //step #4
            var resultEncoded = Base64Encode(result);

            return resultEncoded;

        }

        public string TheIncredibleHulk(string[] input)
        {
            var result = "";
            // step #1
            string[] newWords = GetMovedVowelsArray(input);

            // step #2
            Array.Sort(newWords);
            Array.Reverse(newWords);

            // step #3
            result = string.Join("*", newWords);

            //step #4
            var resultEncoded = Base64Encode(result);

            return resultEncoded;

        }

        public string Thor(string[] input, int fib)
        {
            var result = "";

            // step #1
            string[] words = SeparateEnglishWords(input);
          
	        // step #2
            Array.Sort(words);
           
            // step #3
            string[] newWords = AlternateConstants(words);

	        // step #4
	        var fiboWords = ReplacedVowelsWithFibonacci(newWords, fib);
	        
	        // step #5
	        result = string.Join("*", fiboWords);

	        //step #6
	         var resultEncoded = Base64Encode(result);

	        return resultEncoded;
        }

        public string CaptainAmerica(string[] input, int fib)
        {
            // step #1
            string[] words = GetMovedVowelsArray(input);

            // step #2
            Array.Sort(words);
            Array.Reverse(words);
            // step #3
            var fiboWords = ReplacedVowelsWithFibonacci(words, fib);

            // step #4
            var result = ConcatenateAscci(fiboWords);

            //step #5
            var resultEncoded = Base64Encode(result);

            return resultEncoded;
        }

        public string[] ReplacedVowelsWithFibonacci(string[] words, int fib)
        {
            var result = string.Join("|", words);
            var strBldr = new StringBuilder(result);
            var index = fibo.FindIndex(a => a == fib);
            string newWords = "";
            foreach (var letter in result)
            {
                if (validVowels.Contains(letter))
                {
                    newWords += fibo[index];
                    index++;
                }
                else
                {
                    newWords += letter;
                }
            }


            return newWords.Split('|');
        }

 

        public string[] SeparateEnglishWords(string [] input)
        {
            List<string> newWords = new List<string>();

            foreach (var item in input)
            {

                if (ContainsMoreThanOneEnglishWord(item))
                {
                    var word = "";
                    for (int i = 0; i < item.Length; i++)
                    {
                        word += item[i];
                        if (englishWords.Contains(word.ToLower()))
                        {
                            newWords.Add(word);
                            word = "";
                        }
                    }


                }
                else
                {
                    newWords.Add(item);
                }
            }
            
            return newWords.ToArray();
        }


        public bool ContainsMoreThanOneEnglishWord(string word)
        {
            int counter =0;
            foreach (var englishWord in englishWords)
            {
                if (word.ToLower().Contains(englishWord)) 
                     counter++;
            }

            if (counter > 1)
                return true;
            
            return false;
        }

        public string [] AlternateConstants(string [] input)
        {
            string[] newWords = input;
            var initialCase = char.IsUpper(input[0][0]);
            var alternate = initialCase;

            for(var i =0;i<newWords.Length;i++){
            
            	for(var j = 0;j< newWords[i].Length;j++)
            	{

                    if (!validVowels.Contains(newWords[i][j]))
                    {
            			if(alternate)
            			{
                            var sbl = new StringBuilder(newWords[i]);
            				string cons = newWords[i][j].ToString().ToUpper();
            				sbl[j] = Convert.ToChar(cons);
            				newWords[i] = sbl.ToString();
            				alternate =false;
            			}else{
                            var sbl = new StringBuilder(newWords[i]);
                            string cons = newWords[i][j].ToString().ToLower();
                            sbl[j] = Convert.ToChar(cons);
                            newWords[i] = sbl.ToString();
            				alternate = true;
            			}
            		}
            	}
             
            }
            return newWords;
        }


        public List<long> FibonacciList(int num)
        {
            var fib = new List<long> {0, 1};
            for (var i = 2; i < num; i++)
                fib.Add(fib[i - 1] + (fib[i - 2]));

            return fib;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


        public string ConcatenateAscci(string[] words)
        {
            var result = "";
            result = words[0] + (int)words[words.Length - 1][0];
            for (var i = 1; i < words.Length; i++)
            {
                
                result += words[i] + (int)words[i -1][0];
            }
            return result;
        }


        public string[] GetMovedVowelsArray(string[] input)
        {
            string[] newWords = input;
            for (var i = 0; i < input.Length; i++)
                newWords[i] = MoveOneVowelToTheRight(input[i]);

            return newWords;
        }


        public string MoveOneVowelToTheRight(string word)
        {
            var newWord = word;
            for (var i = 0; i < word.Length; i++)
            {
                if (i < word.Length - 1)
                {
                    if (validVowels.Contains(word[i]) && !validVowels.Contains(word[i + 1]))
                    {
                        newWord = Swap(newWord, i, i + 1);
                    }
                    else if (validVowels.Contains(word[i]) && validVowels.Contains(word[i + 1]))
                    {
                        newWord = Swap(newWord, i, i + 1);
                        i++;
                    }

                }
                else
                {
                    if (validVowels.Contains(word[i]))
                        newWord = Swap(newWord, i, i + 1);

                    break;
                }
            }

            return newWord;
        }


        public string Swap(string value, int origin, int dest)
        {

            string newStr = "";
            if (dest >= value.Length)
            {

                newStr = value[dest - 1] + value.Substring(0, value.Length - 1);
            }
            else
            {
                newStr = value;
                var temp = newStr[dest];
                var temp2 = newStr[origin];
                var sb = new StringBuilder(value);
                sb[origin] = temp;
                sb[dest] = temp2;
                newStr = sb.ToString();
            }
            return newStr;
        }


    }
}