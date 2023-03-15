using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.ExtendedProperties;
using System.Text.RegularExpressions;

namespace WordResumeParser
{
    public class ResumeParser
    {
        string _path = string.Empty;
        List<String> _lines = new List<string>();
        Resume _resume = new Resume();

        public ResumeParser(string path)
        {
            _path = path;

        }

        public void Parse()
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(_path, true))
            {
                MainDocumentPart? mPart = doc.MainDocumentPart;
                if (mPart == null)
                {
                    return;
                }
                using (StreamReader reader = new StreamReader(mPart.GetStream()))
                {
                    XDocument xDocument = XDocument.Load(XmlReader.Create(reader));
                    xDocument.Save("c:\\code\\rh.xml");
                    XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
                    // the elements we will be looking for data.
                    XName rPr = w + "pPr";
                    XName p = w + "p";

#pragma warning disable CS8601 // Possible null reference assignment.
                    var query = from element in xDocument.Descendants(p)
                                select new ProcessedResume
                                {
                                    ItemProperty = element.Element(rPr) != null ?
                                   ((from sElement in element.Descendants(rPr)
                                     select new ItemProperty
                                     {
                                         Style = sElement.IsEmpty == false ?
                                         (sElement.Element(w + "pStyle") != null ?
                     sElement.Element(w + "pStyle")?.Attribute(w + "val")?.Value :
                    string.Empty) : string.Empty,
                                         Lang = sElement.IsEmpty == false ?
                                         (sElement.Element(w + "lang") != null ?
                    (sElement.Element(w + "lang")?.Value ?? string.Empty) :
                    string.Empty) : string.Empty
                                     }).First<ItemProperty>()) : null,
                                    Text = element.Value == string.Empty ? "<br/>"
                    : element.Value

                                };
#pragma warning restore CS8601 // Possible null reference assignment.

                    var resumes =  query.ToList<ProcessedResume>();

                    
                    foreach (ProcessedResume resume in resumes)
                    {
                        Console.WriteLine((resume.ItemProperty?.Style ?? "") + ":" + (resume.Text?? ""));
                        _lines.Add((resume.ItemProperty?.Style ?? "") + ":" + (resume.Text ?? ""));
                    }
                    // File.WriteAllLines("C:\\code\\parsedRes.txt", lines);
                    ProcessResumeLines();
                }
            }
        }

        private void ProcessResumeLines()
        {
            foreach(var line in _lines)
            {
                if (line.Replace("<br/>", "").Replace(":", "").Trim().Length == 0)
                    continue;
                var lineTrimmed = line.Trim().Substring(1); // 0 if always ':'
                var partsOfLine = line.Split(':');
                if(partsOfLine.Length == 2)
                {
                    var header = partsOfLine[0];
                    var detail = partsOfLine[1];
                    ProcessDetailLine(header, detail);
                }
                else if (partsOfLine.Length == 1)
                {
                    ProcessDetailLine("", partsOfLine[0]);
                }
                else
                {
                    throw new ApplicationException("Parts of line more than 2");
                }

            }
        }

        void ProcessDetailLineWithHeader(string header, string detail)
        {

        }

        void ProcessDetailLineWithoutHeader(Enum LineTypeEnum, string detail)
        {

        }

        void ProcessDetailLine(string header, string detail)
        {
            if (header.Length == 0)
            {
                //Experience (or) Experience detail (or) name
                if (detail.Length < 30)
                {
                    //mostly name
                    String[] nameParts = Regex.Split(detail, @" ?");
                    _resume.FirstName = nameParts[0];
                    _resume.LastName = nameParts[1];
                }
                else if (detail.Length < 30)
                {
                    //mostly experience title
                    _resume.Summary= detail;
                }
                else
                {
                    //experience
                }
            }
            else
            {
                // Cell no, Email id, LinkedIn, Heading1 (summary/professional Experience), EDUCATION/CERTIFICATIONS/OPENSOURCE ,
                if(header.IndexOf("Cell no") != -1)
                {
                    _resume.Phone= detail;
                }
            }

        }
    } 
}
