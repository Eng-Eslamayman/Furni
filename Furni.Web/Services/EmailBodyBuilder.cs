namespace Furni.Web.Services
{
    public class EmailBodyBuilder : IEmailBodyBuilder
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmailBodyBuilder(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string GetEmailBody(string template, Dictionary<string, string> placeHolders)
        {
            var filePath = $"{_webHostEnvironment.WebRootPath}/templates/{template}.html";

            StreamReader str = new(filePath);

            var templateContent = str.ReadToEnd();
            str.Close();

            // URL of my App
            //var url = Url.Action("Index", "Home", null, Request.Scheme);

            foreach (var placeHolder in placeHolders)
            {
                templateContent = templateContent.Replace($"[{placeHolder.Key}]", placeHolder.Value);
            }

            return templateContent;
        }
    }
}
