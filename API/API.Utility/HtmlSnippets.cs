namespace API.Utility
{
    public static class HtmlSnippets
    {
        public static string CriarEmailTemplateHtml(string nomeArquivo, Dictionary<string, string> chaveValor)
        {
            var arquivo = File.ReadAllText("Templates/" + nomeArquivo);

            foreach (var item in chaveValor)
                arquivo = arquivo.Replace(item.Key, item.Value);

            return arquivo;
        }
    }
}
