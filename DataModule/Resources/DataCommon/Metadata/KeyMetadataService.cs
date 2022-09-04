using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCommon.DatabaseMetadata
{
    public class KeyMetadataService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReferenceKey"></param>
        /// <param name="SourceKey"></param>
        /// <returns></returns>
        public string[] CreateForeignKey(string Provider, string ReferenceKey, string SourceKey, string OnDelete, string OnUpdate)
        {

            //CASCADE
            if (string.IsNullOrEmpty(Provider)) throw new Exception("Аргумент Provider содержит нулевые данные либо нулевые ссылки");
            if (string.IsNullOrEmpty(ReferenceKey)) throw new Exception("Аргумент ReferenceKey содержит нулевые данные либо нулевые ссылки");
            if (string.IsNullOrEmpty(SourceKey)) throw new Exception("Аргумент SourceKey содержит нулевые данные либо нулевые ссылки");

            var Reference = SplitSingle(ReferenceKey,"-");
            var Source = SplitSingle(SourceKey,"-");
            
            string ReferenceTableName = RemoveTokens(Reference.Key,"[", "]");
            string SourceTableName = RemoveTokens(Source.Key,"[", "]");
           
            if (ReferenceTableName.IsTsqlStyled() == false) throw new Exception("Проверьте аргумент ReferenceKey");
            if (SourceTableName.IsTsqlStyled() == false) throw new Exception("Проверьте аргумент SourceKey");
            switch (Provider)
            {
                case "SqlServer":                    
                    return new string[]{
                        $@" ALTER TABLE {Reference.Key} WITH CHECK ADD CONSTRAINT [FK__{ReferenceTableName}_{SourceTableName}=>{Source.Value}{Source.Key}] FOREIGN KEY([{Source.Value}])
                           REFERENCES {Source.Key} ({Source.Value})
                           ON DELETE {OnDelete} ON UPDATE {OnUpdate}",
                        $@" ALTER TABLE [dbo].[Authors] CHECK CONSTRAINT [FK_Authors_Resources_ResourceID]"
                    };
                case "MySql":
                    throw new NotImplementedException();
                case "Postgre":
                    throw new NotImplementedException();
                default: throw new Exception($"Аргумент Provider может принимать значения: SqlServer,MySql,Postgre, но никак не "+Provider);
            }
        }

        private string RemoveTokens(string key, string v1, string v2)
        {
            if(key.StartsWith(v1))
                key = key.Substring(v1.Length);
            if (key.EndsWith(v2))
                key = key.Substring(0,key.Length-v2.Length);
            return key;
        }

        private KeyValuePair<string,string> SplitSingle(string text, string separator)
        {
            return new KeyValuePair<string,string>(text.Substring(0, text.IndexOf(separator)), text.Substring(text.IndexOf(separator)+separator.Length));    
        }
    }
}
