﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunfrogShirts
{
    class CoreLibary
    {
        //DataJSon
        //{
        //  "ArtOwnerID":0
        //  ,"IAgree":true
        //  ,"Title":"Good for boy"
        //  ,"Category":"76"
        //  ,"Description":"THis for..."
        //  ,"Collections":""
        //  ,"Keywords":["boy","man"]
        //  ,"imageFront":"<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" id=\"SvgjsSvg1000\" version=\"1.1\" width=\"2400\" height=\"3200\" viewBox=\"311.00000000008 150 387.99999999984004 517.33333333312\"><g id=\"SvgjsG1052\" transform=\"scale(0.08399999999996445 0.08399999999996445) translate(3761.9047619073062 2165.0793650801543)\"><image id=\"SvgjsImage1053\" xlink:href=\"__dataURI:0__\" width=\"4500\" height=\"5400\"></image></g><defs id=\"SvgjsDefs1001\"></defs></svg>"
        //  ,"imageBack":""
        //  ,"types":[
        //      {"id":8,"name":"Guys Tee","price":19,"colors":["Orange","Yellow"]}
        //      ,{"id":19,"name":"Hoodie","price":34,"colors":["White","Green"]}
        //  ]
        //  ,"images":[{"id":"__dataURI:0__","uri":"data:image/png;base64,"}]}

        public static string ConvertImageToBase64(string path)
        {
            using (Image image = Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }
    }
}
