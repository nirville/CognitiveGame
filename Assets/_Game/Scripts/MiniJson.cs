using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public static class MiniJSON
{
    public static object Deserialize(string json)
    {
        if (json == null) return null;
        return Parser.Parse(json);
    }

    sealed class Parser : IDisposable
    {
        const string WORD_BREAK = "{}[],:\"\t\n\r ";
        StringReader json;
        Parser(string jsonString)
        {
            json = new StringReader(jsonString);
        }

        public static object Parse(string jsonString)
        {
            using (var instance = new Parser(jsonString))
            {
                return instance.ParseValue();
            }
        }

        public void Dispose()
        {
            json.Dispose();
            json = null;
        }

        Dictionary<string, object> ParseObject()
        {
            var table = new Dictionary<string, object>();
            json.Read(); // {
            while (true)
            {
                TOKEN nextToken = NextToken();
                if (nextToken == TOKEN.NONE) return null;
                if (nextToken == TOKEN.CURLY_CLOSE) return table;
                string name = ParseString();
                if (NextToken() != TOKEN.COLON) return null;
                json.Read();
                table[name] = ParseValue();
            }
        }

        List<object> ParseArray()
        {
            var array = new List<object>();
            json.Read(); // [
            while (true)
            {
                TOKEN nextToken = NextToken();
                if (nextToken == TOKEN.NONE) return null;
                if (nextToken == TOKEN.SQUARED_CLOSE) return array;
                array.Add(ParseValue());
            }
        }

        object ParseValue()
        {
            TOKEN nextToken = NextToken();
            switch (nextToken)
            {
                case TOKEN.STRING: return ParseString();
                case TOKEN.NUMBER: return ParseNumber();
                case TOKEN.CURLY_OPEN: return ParseObject();
                case TOKEN.SQUARED_OPEN: return ParseArray();
                case TOKEN.TRUE: json.Read(); json.Read(); json.Read(); return true;
                case TOKEN.FALSE: for (int i = 0; i < 5; i++) json.Read(); return false;
                case TOKEN.NULL: for (int i = 0; i < 4; i++) json.Read(); return null;
                default: return null;
            }
        }

        string ParseString()
        {
            var sb = new StringBuilder();
            json.Read();
            while (true)
            {
                if (json.Peek() == -1) break;
                char c = (char)json.Read();
                if (c == '\\')
                {
                    char esc = (char)json.Read();
                    switch (esc)
                    {
                        case '"': sb.Append('"'); break;
                        case '\\': sb.Append('\\'); break;
                        case '/': sb.Append('/'); break;
                        case 'b': sb.Append('\b'); break;
                        case 'f': sb.Append('\f'); break;
                        case 'n': sb.Append('\n'); break;
                        case 'r': sb.Append('\r'); break;
                        case 't': sb.Append('\t'); break;
                        case 'u':
                            var hex = new char[4];
                            json.Read(hex, 0, 4);
                            sb.Append((char)Convert.ToInt32(new string(hex), 16));
                            break;
                    }
                }
                else if (c == '"') break;
                else sb.Append(c);
            }
            return sb.ToString();
        }

        object ParseNumber()
        {
            string number = NextWord();
            if (number.IndexOf('.') == -1) return long.Parse(number);
            return double.Parse(number);
        }

        enum TOKEN { NONE, CURLY_OPEN, CURLY_CLOSE, SQUARED_OPEN, SQUARED_CLOSE, COLON, COMMA, STRING, NUMBER, TRUE, FALSE, NULL }

        TOKEN NextToken()
        {
            EatWhitespace();
            if (json.Peek() == -1) return TOKEN.NONE;
            char c = (char)json.Peek();
            switch (c)
            {
                case '{': return TOKEN.CURLY_OPEN;
                case '}': return TOKEN.CURLY_CLOSE;
                case '[': return TOKEN.SQUARED_OPEN;
                case ']': return TOKEN.SQUARED_CLOSE;
                case ',': json.Read(); return TOKEN.COMMA;
                case ':': return TOKEN.COLON;
                case '"': return TOKEN.STRING;
                case 't': return TOKEN.TRUE;
                case 'f': return TOKEN.FALSE;
                case 'n': return TOKEN.NULL;
            }
            if (IsNumberChar(c)) return TOKEN.NUMBER;
            return TOKEN.NONE;
        }

        void EatWhitespace()
        {
            while (IsWhiteSpace(json.Peek())) json.Read();
        }

        bool IsWhiteSpace(int c) => c == ' ' || c == '\t' || c == '\n' || c == '\r';
        bool IsNumberChar(char c) => (c >= '0' && c <= '9') || c == '-' || c == '+' || c == '.' || c == 'e' || c == 'E';

        string NextWord()
        {
            var sb = new StringBuilder();
            while (json.Peek() != -1 && !IsWhiteSpace(json.Peek()) && WORD_BREAK.IndexOf((char)json.Peek()) == -1)
            {
                sb.Append((char)json.Read());
            }
            return sb.ToString();
        }
    }
}