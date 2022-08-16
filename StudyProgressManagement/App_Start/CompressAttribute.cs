using System;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

public class CompressAttribute : ActionFilterAttribute
{
    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        // Implement HTTP compression  
        HttpApplication app = (HttpApplication)sender;

        // Retrieve accepted encodings  
        string encodings = app.Request.Headers.Get("Accept-Encoding");
        if (encodings != null)
        {
            // Check the browser accepts deflate or gzip (deflate takes preference)  
            encodings = encodings.ToLower();
            if (encodings.Contains("gzip"))
            {
                app.Response.Filter = new GZipStream(app.Response.Filter, CompressionMode.Compress);
                app.Response.AppendHeader("Content-Encoding", "gzip");
            }
            else if
                (encodings.Contains("deflate"))
            {
                app.Response.Filter = new DeflateStream(app.Response.Filter, CompressionMode.Compress);
                app.Response.AppendHeader("Content-Encoding", "deflate");
            }
        }
    }
}