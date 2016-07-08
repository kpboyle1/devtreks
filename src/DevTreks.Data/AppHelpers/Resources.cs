using DevTreks.Data.Helpers;
using DevTreks.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DevTreks.Data.AppHelpers
{
    /// <summary>
    ///Purpose:		Support class holding constants, enums, and common methods 
    ///             for pictures, stylesheets, schemas, videos, and audios
    ///Author:		www.devtreks.org
    ///Date:		2016, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///             1. 
    /// </summary>
    public class Resources
    {
        public Resources()
        {
        }
        //standard uripath to a uri's supporting resources
        //resources can be shared by any uri, so they go in a 'shared'
        //uripath and filesystem folder
        //i.e. https://www.devtreks.org/Content/Resource/network_crops/resourcepack_3/resource_6/wheat.svg
        public static string URI_TEMPLATE_PATH = string.Concat("{contentsubfolder}/",
            "{resourcessubfolder}/{networkpart}/{resourcepackpart}/",
            "{resourcepart}/{filename}");
        
        public enum RESOURCES_TYPES
        {
            servicebase     = 0,
            resourcetype    = 1,
            resourcegroup   = 2,
            resourcepack    = 3,
            resource        = 4
        }
        public enum GENERAL_RESOURCE_TYPES
        {
            none            = 0,
            image           = 1,
            thumbnail       = 2,
            video           = 3,
            audio           = 4,
            text            = 5,
            rtf             = 6,
            xml             = 7,
            pdf             = 8,
            stylesheet1     = 9,
            stylesheet2     = 10,
            html            = 11,
            microsoft_ebook = 12,
            idpf_ebook      = 13,
            dtncx_ebook     = 14,
            zip             = 15,
            linkedlists     = 16,
            schema          = 17
        }
        public enum FILEEXTENSION_TYPES
        {
            none =	0,
            aif	=	1,
            aifc	=	2,
            aiff	=	3,
            asc	=	4,
            au	=	5,
            avi	=	6,
            bmp	=	7,
            cgm	=	8,
            css	=	9,
            csv	=	10,
            dif	=	11,
            djv	=	12,
            djvu	=	13,
            doc	=	14,
            dtd	=	15,
            dv	=	16,
            gif	=	17,
            htm	=	18,
            html	=	19,
            ico	=	20,
            ics	=	21,
            ief	=	22,
            ifb	=	23,
            jp2	=	24,
            jpe	=	25,
            jpeg	=	26,
            jpg	=	27,
            js	=	28,
            kar	=	29,
            m3u	=	30,
            m4a	=	31,
            m4b	=	32,
            m4p	=	33,
            m4u	=	34,
            m4v	=	35,
            mac	=	36,
            mid	=	37,
            midi	=	38,
            mov	=	39,
            movie	=	40,
            mp2	=	41,
            mp3	=	42,
            mp4	=	43,
            mpe	=	44,
            mpeg	=	45,
            mpg	=	46,
            mpga	=	47,
            mxu	=	48,
            ncx	=	49,
            pbm	=	50,
            pct	=	51,
            pdf	=	52,
            pgm	=	53,
            pgn	=	54,
            pic	=	55,
            pict	=	56,
            png	=	57,
            pnm	=	58,
            pnt	=	59,
            pntg	=	60,
            ppm	=	61,
            ppt	=	62,
            qt	=	63,
            qti	=	64,
            qtif	=	65,
            ra	=	66,
            ram	=	67,
            ras	=	68,
            rdf	=	69,
            rgb	=	70,
            rtf	=	71,
            rtx	=	72,
            snd	=	73,
            svg	=	74,
            tif	=	75,
            tiff	= 76,
            tsv	=	77,
            txt	=	78,
            wav	=	79,
            wbmp	= 80,
            xbm	=	81,
            xht	=	82,
            xhtml	= 83,
            xls	=	84,
            xml	=	85,
            xpm	=	86,
            xsl	=	87,
            xslt	= 88,
            zip	=	89,
            frag    = 90,
            wmv     = 91,
            gvi     = 92,
            xsd     = 93,
            webm    = 94,
            asf     = 95,
            rm      = 96,
            flv     = 97,
            swf     = 98
        }
        
        public enum PACKAGE_TYPES
        {
            none                = 0,
            //plain zip
            plainzip            = 1,
            //microsoft and ecma
            openxml             = 2,
            //international digital publishing forum's open package format
            idpf                = 3,
            //nimas
            nimas               = 4,
            //oracle?
            opendoc             = 5
        }
        //stylesheet extension objects available
        public enum DISPLAY_NAMESPACE_TYPES
        {
            displaydevpacks     = 0,
            displaycomps        = 1,
            devtreksuri         = 2
        }
        public enum RESOURCES_GETBY_TYPES
        {
            none                = 0,
            resourcetype        = 1,
            resourcepackid      = 2,
            tempdocs            = 3,
            storyuri            = 4,
            storyuriandtagname  = 5,
            storyuriandlabel    = 6,
            filename            = 7
        }
        public static Dictionary<string, string> GetResourceGeneralType()
        {
            Dictionary<string, string> docstats = new Dictionary<string, string>();
            docstats.Add(GENERAL_RESOURCE_TYPES.none.ToString(), GENERAL_RESOURCE_TYPES.none.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.audio.ToString(), GENERAL_RESOURCE_TYPES.audio.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.dtncx_ebook.ToString(), GENERAL_RESOURCE_TYPES.dtncx_ebook.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.html.ToString(), GENERAL_RESOURCE_TYPES.html.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.idpf_ebook.ToString(), GENERAL_RESOURCE_TYPES.idpf_ebook.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.image.ToString(), GENERAL_RESOURCE_TYPES.image.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.linkedlists.ToString(), GENERAL_RESOURCE_TYPES.linkedlists.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.microsoft_ebook.ToString(), GENERAL_RESOURCE_TYPES.microsoft_ebook.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.pdf.ToString(), GENERAL_RESOURCE_TYPES.pdf.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.rtf.ToString(), GENERAL_RESOURCE_TYPES.rtf.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.schema.ToString(), GENERAL_RESOURCE_TYPES.schema.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.stylesheet1.ToString(), GENERAL_RESOURCE_TYPES.stylesheet1.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.stylesheet2.ToString(), GENERAL_RESOURCE_TYPES.stylesheet2.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.text.ToString(), GENERAL_RESOURCE_TYPES.text.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.thumbnail.ToString(), GENERAL_RESOURCE_TYPES.thumbnail.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.video.ToString(), GENERAL_RESOURCE_TYPES.video.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.xml.ToString(), GENERAL_RESOURCE_TYPES.xml.ToString());
            docstats.Add(GENERAL_RESOURCE_TYPES.zip.ToString(), GENERAL_RESOURCE_TYPES.zip.ToString());
            return docstats;
        }
        public static Dictionary<string, string> GetResourceType(ContentURI uri)
        {
            Dictionary<string, string> colTypes = new Dictionary<string, string>();
            if (uri.URIModels.ResourceType != null)
            {
                foreach (var type in uri.URIModels.ResourceType)
                {
                    //note that on the client the key becomes the option's value
                    colTypes.Add(type.PKId.ToString(), type.Name);
                }
            }
            return colTypes;
        }
        public static string GetMimeTypeFromFileExt(ContentURI uri,
            string fileExt)
        {
            string sMimeType = string.Empty;
            //FILEEXTENSION_TYPES eFileExt = (fileExt != string.Empty && fileExt != null) ? (FILEEXTENSION_TYPES)Enum.Parse(typeof(FILEEXTENSION_TYPES), fileExt) : FILEEXTENSION_TYPES.none;
            string sErrorMsg = string.Empty;
            sMimeType = GetMimeTypeFromFileExt(fileExt, ref sErrorMsg);
            uri.ErrorMessage = sErrorMsg;
            return sMimeType;
        }
        public static string GetMimeTypeFromFileExt(string fileExt, ref string errorMsg)
        {
            string sMimeType = string.Empty;
            fileExt = (fileExt.StartsWith(".")) ? fileExt.Remove(0, 1) : fileExt;
            fileExt = fileExt.ToLower();
            if (fileExt == FILEEXTENSION_TYPES.aif.ToString()) sMimeType = "audio/x-aiff";
            else if (fileExt == FILEEXTENSION_TYPES.aifc.ToString()) sMimeType = "audio/x-aiff";
            else if (fileExt == FILEEXTENSION_TYPES.aiff.ToString()) sMimeType = "audio/x-aiff";
            else if (fileExt == FILEEXTENSION_TYPES.asc.ToString()) sMimeType = "text/plain";
            else if (fileExt == FILEEXTENSION_TYPES.au.ToString()) sMimeType = "audio/basic";
            else if (fileExt == FILEEXTENSION_TYPES.avi.ToString()) sMimeType = "video/x-msvideo";
            else if (fileExt == FILEEXTENSION_TYPES.bmp.ToString()) sMimeType = "image/bmp";
            else if (fileExt == FILEEXTENSION_TYPES.cgm.ToString()) sMimeType = "image/cgm";
            else if (fileExt == FILEEXTENSION_TYPES.css.ToString()) sMimeType = "text/css";
            else if (fileExt == FILEEXTENSION_TYPES.csv.ToString()) sMimeType = "text/plain";
            else if (fileExt == FILEEXTENSION_TYPES.dif.ToString()) sMimeType = "video/x-dv";
            else if (fileExt == FILEEXTENSION_TYPES.djv.ToString()) sMimeType = "image/vnd.djvu";
            else if (fileExt == FILEEXTENSION_TYPES.djvu.ToString()) sMimeType = "image/vnd.djvu";
            else if (fileExt == FILEEXTENSION_TYPES.doc.ToString()) sMimeType = "application/msword";
            else if (fileExt == FILEEXTENSION_TYPES.dtd.ToString()) sMimeType = "application/xml-dtd";
            else if (fileExt == FILEEXTENSION_TYPES.dv.ToString()) sMimeType = "video/x-dv";
            else if (fileExt == FILEEXTENSION_TYPES.gif.ToString()) sMimeType = "image/gif";
            else if (fileExt == FILEEXTENSION_TYPES.htm.ToString()) sMimeType = "text/html";
            else if (fileExt == FILEEXTENSION_TYPES.html.ToString()) sMimeType = "text/html";
            else if (fileExt == FILEEXTENSION_TYPES.ico.ToString()) sMimeType = "image/x-icon";
            else if (fileExt == FILEEXTENSION_TYPES.ics.ToString()) sMimeType = "text/calendar";
            else if (fileExt == FILEEXTENSION_TYPES.ief.ToString()) sMimeType = "image/ief";
            else if (fileExt == FILEEXTENSION_TYPES.ifb.ToString()) sMimeType = "text/calendar";
            else if (fileExt == FILEEXTENSION_TYPES.jp2.ToString()) sMimeType = "image/jp2";
            else if (fileExt == FILEEXTENSION_TYPES.jpe.ToString()) sMimeType = "image/jpeg";
            else if (fileExt == FILEEXTENSION_TYPES.jpeg.ToString()) sMimeType = "image/jpeg";
            else if (fileExt == FILEEXTENSION_TYPES.jpg.ToString()) sMimeType = "image/jpeg";
            else if (fileExt == FILEEXTENSION_TYPES.js.ToString()) sMimeType = "application/x-javascript";
            else if (fileExt == FILEEXTENSION_TYPES.kar.ToString()) sMimeType = "audio/midi";
            else if (fileExt == FILEEXTENSION_TYPES.m3u.ToString()) sMimeType = "audio/x-mpegurl";
            else if (fileExt == FILEEXTENSION_TYPES.m4a.ToString()) sMimeType = "audio/mp4a-latm";
            else if (fileExt == FILEEXTENSION_TYPES.m4b.ToString()) sMimeType = "audio/mp4a-latm";
            else if (fileExt == FILEEXTENSION_TYPES.m4p.ToString()) sMimeType = "audio/mp4a-latm";
            else if (fileExt == FILEEXTENSION_TYPES.m4u.ToString()) sMimeType = "video/vnd.mpegurl";
            else if (fileExt == FILEEXTENSION_TYPES.m4v.ToString()) sMimeType = "video/x-m4v";
            else if (fileExt == FILEEXTENSION_TYPES.mac.ToString()) sMimeType = "image/x-macpaint";
            else if (fileExt == FILEEXTENSION_TYPES.mid.ToString()) sMimeType = "audio/midi";
            else if (fileExt == FILEEXTENSION_TYPES.midi.ToString()) sMimeType = "audio/midi";
            else if (fileExt == FILEEXTENSION_TYPES.mov.ToString()) sMimeType = "video/quicktime";
            else if (fileExt == FILEEXTENSION_TYPES.movie.ToString()) sMimeType = "video/x-sgi-movie";
            else if (fileExt == FILEEXTENSION_TYPES.mp2.ToString()) sMimeType = "audio/mpeg";
            else if (fileExt == FILEEXTENSION_TYPES.mp3.ToString()) sMimeType = "audio/mpeg";
            else if (fileExt == FILEEXTENSION_TYPES.mp4.ToString()) sMimeType = "video/mp4";
            else if (fileExt == FILEEXTENSION_TYPES.mpe.ToString()) sMimeType = "video/mpeg";
            else if (fileExt == FILEEXTENSION_TYPES.mpeg.ToString()) sMimeType = "video/mpeg";
            else if (fileExt == FILEEXTENSION_TYPES.mpg.ToString()) sMimeType = "video/mpeg";
            else if (fileExt == FILEEXTENSION_TYPES.mpga.ToString()) sMimeType = "audio/mpeg";
            else if (fileExt == FILEEXTENSION_TYPES.mxu.ToString()) sMimeType = "video/vnd.mpegurl";
            else if (fileExt == FILEEXTENSION_TYPES.ncx.ToString()) sMimeType = "application/x-dtbncx+xml";
            else if (fileExt == FILEEXTENSION_TYPES.pbm.ToString()) sMimeType = "image/x-portable-bitmap";
            else if (fileExt == FILEEXTENSION_TYPES.pct.ToString()) sMimeType = "image/pict";
            else if (fileExt == FILEEXTENSION_TYPES.pdf.ToString()) sMimeType = "application/pdf";
            else if (fileExt == FILEEXTENSION_TYPES.pgm.ToString()) sMimeType = "image/x-portable-graymap";
            else if (fileExt == FILEEXTENSION_TYPES.pgn.ToString()) sMimeType = "application/x-chess-pgn";
            else if (fileExt == FILEEXTENSION_TYPES.pic.ToString()) sMimeType = "image/pict";
            else if (fileExt == FILEEXTENSION_TYPES.pict.ToString()) sMimeType = "image/pict";
            else if (fileExt == FILEEXTENSION_TYPES.png.ToString()) sMimeType = "image/png";
            else if (fileExt == FILEEXTENSION_TYPES.pnm.ToString()) sMimeType = "image/x-portable-anymap";
            else if (fileExt == FILEEXTENSION_TYPES.pnt.ToString()) sMimeType = "image/x-macpaint";
            else if (fileExt == FILEEXTENSION_TYPES.pntg.ToString()) sMimeType = "image/x-macpaint";
            else if (fileExt == FILEEXTENSION_TYPES.ppm.ToString()) sMimeType = "image/x-portable-pixmap";
            else if (fileExt == FILEEXTENSION_TYPES.ppt.ToString()) sMimeType = "application/vnd.ms-powerpoint";
            else if (fileExt == FILEEXTENSION_TYPES.qt.ToString()) sMimeType = "video/quicktime";
            else if (fileExt == FILEEXTENSION_TYPES.qti.ToString()) sMimeType = "image/x-quicktime";
            else if (fileExt == FILEEXTENSION_TYPES.qtif.ToString()) sMimeType = "image/x-quicktime";
            else if (fileExt == FILEEXTENSION_TYPES.ra.ToString()) sMimeType = "audio/x-pn-realaudio";
            else if (fileExt == FILEEXTENSION_TYPES.ram.ToString()) sMimeType = "audio/x-pn-realaudio";
            else if (fileExt == FILEEXTENSION_TYPES.ras.ToString()) sMimeType = "image/x-cmu-raster";
            else if (fileExt == FILEEXTENSION_TYPES.rdf.ToString()) sMimeType = "application/rdf+xml";
            else if (fileExt == FILEEXTENSION_TYPES.rgb.ToString()) sMimeType = "image/x-rgb";
            else if (fileExt == FILEEXTENSION_TYPES.rtf.ToString()) sMimeType = "text/rtf";
            else if (fileExt == FILEEXTENSION_TYPES.rtx.ToString()) sMimeType = "text/richtext";
            else if (fileExt == FILEEXTENSION_TYPES.snd.ToString()) sMimeType = "audio/basic";
            else if (fileExt == FILEEXTENSION_TYPES.svg.ToString()) sMimeType = "image/svg+xml";
            else if (fileExt == FILEEXTENSION_TYPES.tif.ToString()) sMimeType = "image/tiff";
            else if (fileExt == FILEEXTENSION_TYPES.tiff.ToString()) sMimeType = "image/tiff";
            else if (fileExt == FILEEXTENSION_TYPES.tsv.ToString()) sMimeType = "text/tab-separated-values";
            else if (fileExt == FILEEXTENSION_TYPES.txt.ToString()) sMimeType = "text/plain";
            else if (fileExt == FILEEXTENSION_TYPES.wav.ToString()) sMimeType = "audio/x-wav";
            else if (fileExt == FILEEXTENSION_TYPES.wbmp.ToString()) sMimeType = "image/vnd.wap.wbmp";
            else if (fileExt == FILEEXTENSION_TYPES.xbm.ToString()) sMimeType = "image/x-xbitmap";
            else if (fileExt == FILEEXTENSION_TYPES.xht.ToString()) sMimeType = "application/xhtml+xml";
            else if (fileExt == FILEEXTENSION_TYPES.xhtml.ToString()) sMimeType = "application/xhtml+xml";
            else if (fileExt == FILEEXTENSION_TYPES.xls.ToString()) sMimeType = "application/vnd.ms-excel";
            else if (fileExt == FILEEXTENSION_TYPES.xml.ToString()) sMimeType = "application/xml";
            else if (fileExt == FILEEXTENSION_TYPES.xpm.ToString()) sMimeType = "image/x-xpixmap";
            else if (fileExt == FILEEXTENSION_TYPES.xsl.ToString()) sMimeType = "application/xml";
            else if (fileExt == FILEEXTENSION_TYPES.xslt.ToString()) sMimeType = "application/xslt+xml";
            else if (fileExt == FILEEXTENSION_TYPES.zip.ToString()) sMimeType = "application/zip";
            else if (fileExt == FILEEXTENSION_TYPES.frag.ToString()) sMimeType = "text/plain";
            else if (fileExt == FILEEXTENSION_TYPES.wmv.ToString()) sMimeType = "video/wmv";
            else if (fileExt == FILEEXTENSION_TYPES.gvi.ToString()) sMimeType = "video/gvi";
            else if (fileExt == FILEEXTENSION_TYPES.xsd.ToString()) sMimeType = "application/xml";
            else if (fileExt == FILEEXTENSION_TYPES.webm.ToString()) sMimeType = "video/webm";
            else if (fileExt == FILEEXTENSION_TYPES.asf.ToString()) sMimeType = "video/asf";
            else if (fileExt == FILEEXTENSION_TYPES.rm.ToString()) sMimeType = "video/rm";
            else if (fileExt == FILEEXTENSION_TYPES.flv.ToString()) sMimeType = "video/flv";
            else if (fileExt == FILEEXTENSION_TYPES.swf.ToString()) sMimeType = "video/swf";
            else
            {
                errorMsg = Exceptions.DevTreksErrors.MakeStandardErrorMsg("No support for: " + fileExt, "RESOURCES_NOFILETYPE");
            }
            return sMimeType;
        }
        public static bool IsResourceImage(ContentURI resource)
        {
            bool bIsImage = false;
            if (resource != null)
            {
                bIsImage = IsImage(resource.URIDataManager.FileSystemPath);
            }
            return bIsImage;
        }
        public static bool IsImage(string imageFilePath)
        {
            bool bIsImage = false;
            if (string.IsNullOrEmpty(imageFilePath))
            {
                return false;
            }
            string imageFileName = imageFilePath.ToLower();
            //google requires png, gif or jpg
            if (imageFileName.EndsWith(FILEEXTENSION_TYPES.gif.ToString()))
            {
                return true;
            }
           
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.jpg.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.png.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.svg.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.gif.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.jpeg.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.tiff.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.tif.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.ico.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.bmp.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.cgm.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.djv.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.djvu.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.jpe.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.jp2.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.mac.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.pbm.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.pct.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.pic.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.pict.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.pnm.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.pnt.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.pntg.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.rgb.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.qti.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.qtif.ToString()))
            {
                return true;
            }
            else if (imageFileName.EndsWith(FILEEXTENSION_TYPES.ico.ToString()))
            {
                return true;
            }
            return bIsImage;
        }
        public static bool IsResourceVideo(ContentURI resource)
        {
            bool bIsVideo = false;
            if (resource != null)
            {
                bIsVideo = IsVideo(resource.URIDataManager.FileSystemPath);
            }
            return bIsVideo;
        }
        public static bool IsVideo(string videoFilePath)
        {
            bool bIsVideo = false;
            if (string.IsNullOrEmpty(videoFilePath))
            {
                return false;
            }
            string videoFileName = videoFilePath.ToLower();
            if (videoFileName.EndsWith(FILEEXTENSION_TYPES.mpg.ToString()))
            {
                return true;
            }
            else if (videoFileName.EndsWith(FILEEXTENSION_TYPES.jpeg.ToString()))
            {
                return true;
            }
            else if (videoFileName.EndsWith(FILEEXTENSION_TYPES.mpeg.ToString()))
            {
                return true;
            }
            else if (videoFileName.EndsWith(FILEEXTENSION_TYPES.mp4.ToString()))
            {
                return true;
            }
            else if (videoFileName.EndsWith(FILEEXTENSION_TYPES.m4v.ToString()))
            {
                return true;
            }
            else if (videoFileName.EndsWith(FILEEXTENSION_TYPES.mov.ToString()))
            {
                return true;
            }
            else if (videoFileName.EndsWith(FILEEXTENSION_TYPES.wmv.ToString()))
            {
                return true;
            }
            else if (videoFileName.EndsWith(FILEEXTENSION_TYPES.asf.ToString()))
            {
                return true;
            }
            else if (videoFileName.EndsWith(FILEEXTENSION_TYPES.avi.ToString()))
            {
                return true;
            }
            else if (videoFileName.EndsWith(FILEEXTENSION_TYPES.ra.ToString()))
            {
                return true;
            }
            else if (videoFileName.EndsWith(FILEEXTENSION_TYPES.ram.ToString()))
            {
                return true;
            }
            else if (videoFileName.EndsWith(FILEEXTENSION_TYPES.rm.ToString()))
            {
                return true;
            }
            else if (videoFileName.EndsWith(FILEEXTENSION_TYPES.flv.ToString()))
            {
                return true;
            }
            else if (videoFileName.EndsWith(FILEEXTENSION_TYPES.swf.ToString()))
            {
                return true;
            }
            return bIsVideo;
        }
        public static bool IsResourceStory(ContentURI resource)
        {
            bool bIsStory = false;
            if (resource != null)
            {
                bIsStory = IsStory(resource.URIDataManager.FileSystemPath);
            }
            return bIsStory;
        }
        public static bool IsStory(string storyFilePath)
        {
            bool bIsStory = false;
            if (string.IsNullOrEmpty(storyFilePath))
            {
                return false;
            }
            string storyFileName = storyFilePath.ToLower();
            if (storyFileName.EndsWith(FILEEXTENSION_TYPES.txt.ToString()))
            {
                return true;
            }
            else if (storyFileName.EndsWith(FILEEXTENSION_TYPES.pdf.ToString()))
            {
                return true;
            }
            return bIsStory;
        }
        public static void SetAppSearchView(string currentNodeName,
            int currentId, ContentURI uri)
        {
            if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
            {
                uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                //checkboxes for node insertions
                uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
            }
            else
            {
                uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
            }
            //link backwards
            uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
            //link forwards
            if (currentNodeName == RESOURCES_TYPES.resource.ToString())
            {
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.none;
            }
            else
            {
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
            }
            //tell ui about children node name (for adding to/selecting from tocs)
            if (currentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString()
                || currentNodeName == Agreement.AGREEMENT_TYPES.service.ToString())
            {
                uri.URIDataManager.ChildrenNodeName = RESOURCES_TYPES.resourcegroup.ToString();
            }
            else if (currentNodeName == RESOURCES_TYPES.resourcegroup.ToString())
            {
                uri.URIDataManager.ChildrenNodeName = RESOURCES_TYPES.resourcepack.ToString();
            }
            else if (currentNodeName == RESOURCES_TYPES.resourcepack.ToString())
            {
                uri.URIDataManager.ChildrenNodeName = RESOURCES_TYPES.resource.ToString();
            }
            else
            {
                uri.URIDataManager.ChildrenNodeName = string.Empty;
            }
        }
        
        public async Task<string> GetAndSaveResourceURLInFilesAsync( 
            ContentURI resourceURI, string resourceURIPath, bool needsFileSystemPath)
        {
            string sResourcePath = string.Empty;
            bool bNeedsWebFullPath = false;
            sResourcePath = GetRootedResourcePath(resourceURI, resourceURIPath, bNeedsWebFullPath, 
                Helpers.GeneralHelpers.FILE_PATH_DELIMITER);
            if (Helpers.FileStorageIO.URIAbsoluteExists(resourceURI,
                sResourcePath) == false)
            {
                //see if it can be retrieved from the db and stored in the proper path
                await StoreResourceInFileSystemByIdAsync(sResourcePath, resourceURI);
                if (resourceURIPath == string.Empty) sResourcePath = string.Empty;
            }
            if (needsFileSystemPath == false
                && resourceURIPath != string.Empty
                && (!FileStorageIO.URIAbsoluteExists(resourceURI,
                    resourceURIPath)))
            {
                //0.8.7 same resource storage for packages and other resources
                //return the relative web path to resource
                bNeedsWebFullPath = true;
                sResourcePath = AppSettings.GetResourceRootPath(resourceURI, bNeedsWebFullPath);
                sResourcePath = string.Concat(sResourcePath, resourceURIPath);
            }
            return sResourcePath;
        }
        public string GetAndSaveResourceURLInFiles(ContentURI resourceURI,
            string resourceURIPath, bool needsFileSystemPath)
        {
            string sResourcePath = string.Empty;
            bool bNeedsWebFullPath = false;
            sResourcePath = GetRootedResourcePath(resourceURI, resourceURIPath, bNeedsWebFullPath,
                Helpers.GeneralHelpers.FILE_PATH_DELIMITER);
            if (Helpers.FileStorageIO.URIAbsoluteExists(resourceURI,
                sResourcePath) == false)
            {
                //ContentViewModel.GetResourceAsync() handles resource state management
                //if the resource is missing that method is not working
                //Task resourcesLVS = StoreResourceInFileSystemByIdAsync(sqlIO, sResourcePath, resourceURI);
                //if (resourceURIPath == string.Empty) sResourcePath = string.Empty;
            }
            if (needsFileSystemPath == false
                && resourceURIPath != string.Empty
                && (!FileStorageIO.URIAbsoluteExists(resourceURI, resourceURIPath)))
            {
                //0.8.7 same resource storage for packages and other resources
                //return the relative web path to resource
                bNeedsWebFullPath = true;
                sResourcePath = AppSettings.GetResourceRootPath(resourceURI, bNeedsWebFullPath);
                sResourcePath = string.Concat(sResourcePath, resourceURIPath);
            }
            return sResourcePath;
        }
        public static string GetResourceURL(ContentURI resourceURI,
            string resourceURIPath, bool needsFileSystemPath)
        {
            string sResourcePath = string.Empty;
            if (needsFileSystemPath == false
                && resourceURIPath != string.Empty)
            {
                //0.8.7 same resource storage for packages and other resources
                //return the relative web path to resource
                bool bNeedsWebFullPath = true;
                sResourcePath = AppSettings.GetResourceRootPath(resourceURI, bNeedsWebFullPath);
                sResourcePath = string.Concat(sResourcePath, resourceURIPath);
            }
            return sResourcePath;
        }
        
        public static string GetRootedResourcePath(ContentURI resourceURI,
            string resourceURIPath, bool needsFullWebPath, 
            string delimiterToUse)
        {
            string sResourceWebPath = string.Empty;
            string sDelimiterToReplace = (delimiterToUse == Helpers.GeneralHelpers.FILE_PATH_DELIMITER)
                ? Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER : Helpers.GeneralHelpers.FILE_PATH_DELIMITER;
            string sPathToResource = AppSettings.GetResourceRootPath(resourceURI, needsFullWebPath);
            
            int iFullPathIndex
                = resourceURIPath.IndexOf(sPathToResource);
            if (iFullPathIndex < 0)
            {
                sResourceWebPath = string.Concat(sPathToResource,
                    resourceURIPath.Replace(sDelimiterToReplace, delimiterToUse));
            }
            else
            {
                sResourceWebPath = resourceURIPath;
            }
            return sResourceWebPath;
        }
        public async Task<SqlDataReader> GetResourceAsync(Helpers.SqlIOAsync sqlIO, ContentURI uri,
            bool needsOneRecord, bool needsFullPath,
            RESOURCES_GETBY_TYPES getResourceByType,
            string getResourceByParam)
        {
            SqlParameter[] oPrams = GetResourceParams(sqlIO, uri, needsOneRecord,
                getResourceByType, getResourceByParam);
            string sQry = GetResourceQry(getResourceByType);
            SqlDataReader resources = await sqlIO.RunProcAsync(sQry,
                oPrams);
            return resources;
        }
        public string GetResourceURLs(ContentURI uri,
            bool needsOneRecord, bool needsFullPath,
            RESOURCES_GETBY_TYPES getResourceByType,
            string getResourceByParam)
        {
            string sResourceFilePaths = string.Empty;
            SqlIOAsync sqlIO = new SqlIOAsync(uri);
            SqlParameter[] oPrams = GetResourceParams(sqlIO, uri, needsOneRecord,
                getResourceByType, getResourceByParam);
            string sQry = GetResourceQry(getResourceByType);
            SqlDataReader oDataReader = sqlIO.RunProc(
                sQry, oPrams);
            if (oDataReader != null && (!oDataReader.IsClosed))
            {
                sResourceFilePaths = GetResourceArrays(uri, needsFullPath, oDataReader);
            }
            sqlIO.Dispose();
            return sResourceFilePaths;
        }
        public async Task<string> GetResourceURLsAsync(ContentURI uri,
            bool needsOneRecord, bool needsFullPath,
            RESOURCES_GETBY_TYPES getResourceByType,
            string getResourceByParam)
        {
            string sResourceFilePaths = string.Empty;
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] oPrams = GetResourceParams(sqlIO, uri, needsOneRecord,
                getResourceByType, getResourceByParam);
            string sQry = GetResourceQry(getResourceByType);
            SqlDataReader oDataReader =  await sqlIO.RunProcAsync( 
                sQry, oPrams);
            if (oDataReader != null && (!oDataReader.IsClosed))
            {
                //closes the reader too
                sResourceFilePaths = await GetResourceArraysAsync(uri, needsFullPath, oDataReader);
            }
            sqlIO.Dispose();
            return sResourceFilePaths;
        }

        private string GetResourceQry(RESOURCES_GETBY_TYPES getResourceByType)
        {
            string sQry = string.Empty;
            switch (getResourceByType)
            {
                case RESOURCES_GETBY_TYPES.resourcetype:
                    sQry = "0GetResourceIdsForPathsByType";
                    break;
                case RESOURCES_GETBY_TYPES.storyuri:
                    bool needsThumbnailsOnly = false;
                    if (needsThumbnailsOnly)
                    {
                        sQry = "0GetResourceIdsForThumbnails";
                    }
                    else
                    {
                        sQry = "0GetResourceIdsForPaths";
                    }
                    break;
                case RESOURCES_GETBY_TYPES.storyuriandlabel:
                    sQry = "0GetResourceIdsByLabel";
                    break;
                case RESOURCES_GETBY_TYPES.storyuriandtagname:
                    sQry = "0GetResourceIdsByTagName";
                    break;
                case RESOURCES_GETBY_TYPES.filename:
                    sQry = "0GetResourceIdsBySearchName";
                    break;
                default:
                    break;
            }
            return sQry;
        }
        private static SqlParameter[] GetResourceParams(Helpers.SqlIOAsync sqlIO,
            ContentURI uri, bool needsOneRecord, RESOURCES_GETBY_TYPES getResourceByType, 
            string getResourceByParam)
        {
            int iParentId = uri.URIId;
            if (uri.URIDataManager.ServerActionType 
                == Helpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews
                || (uri.URIDataManager.ServerActionType 
                    == Helpers.GeneralHelpers.SERVER_ACTION_TYPES.edit
                && uri.URIDataManager.UseSelectedLinkedView))
            {
                if (uri.URIDataManager.BaseId != 0
                    && uri.URIDataManager.AppType != Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
                {
                    iParentId = uri.URIDataManager.BaseId;
                }
            }
            if (getResourceByType == RESOURCES_GETBY_TYPES.filename)
            {
                int iLinkedViewJoinId = 0;
                if (uri.URIDataManager.Resource != null)
                {
                    if (uri.URIDataManager.Resource.Count > 0)
                    {
                        //the join id is used to find the base linkedview id and its related resource
                        iLinkedViewJoinId = uri.URIDataManager.Resource.FirstOrDefault().URIId;
                    }
                }
                SqlParameter[] oPrams =
                {
		            sqlIO.MakeInParam("@Id",				SqlDbType.Int, 4, iParentId),
                    sqlIO.MakeInParam("@ResourceByParam",   SqlDbType.NVarChar, 100, getResourceByParam),
                    sqlIO.MakeInParam("@NodeName",          SqlDbType.NVarChar, 50, uri.URINodeName),
                    sqlIO.MakeInParam("@LinkedViewJoinId",    SqlDbType.Int, 4, iLinkedViewJoinId)
	            };
                return oPrams;
            }
            else 
            {
                SqlParameter[] oPrams =
                {
		            sqlIO.MakeInParam("@Id",				SqlDbType.Int, 4, iParentId),
                    sqlIO.MakeInParam("@ResourceByParam",   SqlDbType.NVarChar, 100, getResourceByParam),
                    sqlIO.MakeInParam("@NodeName",          SqlDbType.NVarChar, 50, uri.URINodeName),
                    sqlIO.MakeInParam("@NeedsOneRecord",    SqlDbType.Bit, 1, needsOneRecord)
	            };
                return oPrams;
            }
        }
        public async Task<string> GetResourceURLsForTempDocsAsync(
            ContentURI uri)
        {
            //called by the packaging services
            string sResourceFilePaths = string.Empty;
            //safest is to load uri.URIClub.ClubDocFullPath xml document
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri, uri.URIClub.ClubDocFullPath))
            {
                XmlReader oTempDocReader 
                    = await Helpers.FileStorageIO.GetXmlReaderAsync(uri, uri.URIClub.ClubDocFullPath);
                if (oTempDocReader != null)
                {
                    using (oTempDocReader)
                    {
                        string sResourcePackNodeName = string.Empty;
                        string sResourcePackId = string.Empty;
                        string sResourcePackName = string.Empty;
                        string sResourcePackURI = string.Empty;
                        string sResourceURIs = string.Empty;
                        //note later might add resourceids to existing elements and this would need to be adjusted
                        if (uri.URIDataManager.AppType == Helpers.GeneralHelpers.APPLICATION_TYPES.resources)
                        {
                            sResourcePackNodeName = RESOURCES_TYPES.resourcepack.ToString();
                        }
                        else if (uri.URIDataManager.AppType == Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
                        {
                            sResourcePackNodeName = DevPacks.DEVPACKS_TYPES.devpackresourcepack.ToString();
                        }
                        else if (uri.URIDataManager.AppType == Helpers.GeneralHelpers.APPLICATION_TYPES.linkedviews)
                        {
                            sResourcePackNodeName = LinkedViews.LINKEDVIEWS_TYPES.linkedviewresourcepack.ToString();
                        }
                        if (sResourcePackNodeName != string.Empty)
                        {
                            //retrieve all of the resourcepack elements from the document
                            while (oTempDocReader.ReadToFollowing(sResourcePackNodeName))
                            {
                                //this assumes the id field is still be a legit db id
                                sResourcePackId = oTempDocReader.GetAttribute(AppHelpers.Calculator.cId);
                                int iResourcePackId = Helpers.GeneralHelpers.ConvertStringToInt(sResourcePackId);
                                sResourcePackName = oTempDocReader.GetAttribute("Name");
                                ContentURI resourcePackURI = new ContentURI(
                                    sResourcePackName, iResourcePackId,
                                    uri.URINetworkPartName, sResourcePackNodeName, uri.URIFileExtensionType);
                                AddParentURIPropertiesToResourceURI(uri, resourcePackURI);
                                bool bNeedsOneRecord = false;
                                //should mean that its cloud compatible
                                bool bNeedsFullPath = false;
                                string sResourceGetByParam = string.Empty;
                                sResourceFilePaths = await GetResourceURLsAsync(uri, bNeedsOneRecord,
                                    bNeedsFullPath, RESOURCES_GETBY_TYPES.storyuri,
                                    sResourceGetByParam);
                                sResourceFilePaths += (sResourceFilePaths.Length > 0) ?
                                    Helpers.GeneralHelpers.PARAMETER_DELIMITER + sResourceURIs : sResourceURIs;
                            }
                        }
                    }
                }
            }
            return sResourceFilePaths;
        }
        private async Task<string> GetResourceArraysAsync(ContentURI uri,
            bool needsFullFilePath, SqlDataReader dataReader)
        {
            string sResourceFilePaths = string.Empty;
            int i = 0;
            string sResourcePath = string.Empty;
            string sFullResourcePath = string.Empty;
            string sResourceAltDescription = string.Empty;
            string sResourceURIPattern = string.Empty;
            StringBuilder oPaths = new StringBuilder();
            if (dataReader != null && (!dataReader.IsClosed))
            {
                using (dataReader)
                {
                    while (await dataReader.ReadAsync())
                    {
                        //sequential data has to be read in order (once an index is passed, can't return to it)
                        //get the media type
                        if (dataReader.IsDBNull(0) == false)
                        {
                            //first param returns a resourceRelPath (same as pagetoc, missing starting full or rel path)
                            sResourcePath = dataReader.GetString(0);
                            ContentURI oResourceURI = GetResourceURIFromResourcePath(uri, sResourcePath);
                            AddParentURIPropertiesToResourceURI(uri, oResourceURI);
                            //only stores the resource in blob if it doesn't exist
                            await GetPathAndStoreResourceAsync(oResourceURI, sResourcePath, needsFullFilePath);
                            sFullResourcePath = oResourceURI.URIDataManager.FileSystemPath;
                        }
                        if (dataReader.IsDBNull(1) == false)
                        {
                            sResourceAltDescription = dataReader.GetString(1);
                            //clean up for use in delimited string
                            sResourceAltDescription = sResourceAltDescription.Replace(Helpers.GeneralHelpers.STRING_DELIMITER,
                                string.Empty);
                            sResourceAltDescription = sResourceAltDescription.Replace(Helpers.GeneralHelpers.PARAMETER_DELIMITER,
                                string.Empty);
                        }
                        if (dataReader.IsDBNull(2) == false)
                        {
                            sResourceURIPattern = dataReader.GetString(2);
                        }
                        if (string.IsNullOrEmpty(sFullResourcePath) == false)
                        {
                            if (i == 0)
                            {
                                oPaths.Append(sFullResourcePath);
                            }
                            else
                            {
                                oPaths.Append(Helpers.GeneralHelpers.PARAMETER_DELIMITER);
                                oPaths.Append(sFullResourcePath);
                            }
                            //alt attribute (for accessability)
                            oPaths.Append(Helpers.GeneralHelpers.STRING_DELIMITER);
                            oPaths.Append(sResourceAltDescription);
                            //name attribute (for caching)
                            oPaths.Append(Helpers.GeneralHelpers.STRING_DELIMITER);
                            oPaths.Append(sResourceURIPattern);
                            //2.0.0 need appsetting connection strings for internal stylesheet calls
                            //2.0.0 refactor this if any security threat (servers must only use dynamically)
                            oPaths.Append(Helpers.GeneralHelpers.STRING_DELIMITER);
                            //db connection
                            //can't use semicolondelimiters because they're same as STRING_DELIMITER
                            string sConnection = uri.URIDataManager.DefaultConnection.Replace(
                                Helpers.GeneralHelpers.STRING_DELIMITER, Helpers.GeneralHelpers.FORMELEMENT_DELIMITER);
                            //can't use @ symbols in passwords (possibly others as well)
                            sConnection = sConnection.Replace(
                                Helpers.GeneralHelpers.PARAMETER_DELIMITER, Helpers.GeneralHelpers.FORMELEMENT_DELIMITER2);
                            oPaths.Append(sConnection);
                            oPaths.Append(Helpers.GeneralHelpers.STRING_DELIMITER);
                            //azure blob storage (fromdelimiter is safe for 2.0.0 connection strings)
                            sConnection = uri.URIDataManager.StorageConnection.Replace(
                                Helpers.GeneralHelpers.STRING_DELIMITER, Helpers.GeneralHelpers.FORMELEMENT_DELIMITER);
                            sConnection = sConnection.Replace(
                                Helpers.GeneralHelpers.PARAMETER_DELIMITER, Helpers.GeneralHelpers.FORMELEMENT_DELIMITER2);
                            oPaths.Append(sConnection);
                            i += 1;
                        }
                        sResourcePath = string.Empty;
                        sFullResourcePath = string.Empty;
                        sResourceAltDescription = string.Empty;
                        sResourceURIPattern = string.Empty;
                    }
                }
            }
            sResourceFilePaths = oPaths.ToString();
            return sResourceFilePaths;
        }
        private string GetResourceArrays(ContentURI uri,
            bool needsFullFilePath, SqlDataReader dataReader)
        {
            string sResourceFilePaths = string.Empty;
            int i = 0;
            string sResourcePath = string.Empty;
            string sFullResourcePath = string.Empty;
            string sResourceAltDescription = string.Empty;
            string sResourceURIPattern = string.Empty;
            StringBuilder oPaths = new StringBuilder();
            if (dataReader != null && (!dataReader.IsClosed))
            {
                using (dataReader)
                {
                    while (dataReader.Read())
                    {
                        //sequential data has to be read in order (once an index is passed, can't return to it)
                        //get the media type
                        if (dataReader.IsDBNull(0) == false)
                        {
                            //first param returns a resourceRelPath (same as pagetoc, missing starting full or rel path)
                            sResourcePath = dataReader.GetString(0);
                            ContentURI oResourceURI = GetResourceURIFromResourcePath(uri, sResourcePath);
                            AddParentURIPropertiesToResourceURI(uri, oResourceURI);
                            //only stores the resource in blob if it doesn't exist -called from ss so no async
                            GetPathAndStoreResource(oResourceURI, sResourcePath, needsFullFilePath);
                            sFullResourcePath = oResourceURI.URIDataManager.FileSystemPath;
                        }
                        if (dataReader.IsDBNull(1) == false)
                        {
                            sResourceAltDescription = dataReader.GetString(1);
                            //clean up for use in delimited string
                            sResourceAltDescription = sResourceAltDescription.Replace(Helpers.GeneralHelpers.STRING_DELIMITER,
                                string.Empty);
                            sResourceAltDescription = sResourceAltDescription.Replace(Helpers.GeneralHelpers.PARAMETER_DELIMITER,
                                string.Empty);
                        }
                        if (dataReader.IsDBNull(2) == false)
                        {
                            sResourceURIPattern = dataReader.GetString(2);
                        }
                        if (string.IsNullOrEmpty(sFullResourcePath) == false)
                        {
                            if (i == 0)
                            {
                                oPaths.Append(sFullResourcePath);
                            }
                            else
                            {
                                oPaths.Append(Helpers.GeneralHelpers.PARAMETER_DELIMITER);
                                oPaths.Append(sFullResourcePath);
                            }
                            //alt attribute (for accessability)
                            oPaths.Append(Helpers.GeneralHelpers.STRING_DELIMITER);
                            oPaths.Append(sResourceAltDescription);
                            //name attribute (for caching)
                            oPaths.Append(Helpers.GeneralHelpers.STRING_DELIMITER);
                            oPaths.Append(sResourceURIPattern);
                            //2.0.0 need appsetting connection strings for internal stylesheet calls
                            oPaths.Append(Helpers.GeneralHelpers.STRING_DELIMITER);
                            //db connection
                            //can't use semicolondelimiters because they're same as STRING_DELIMITER
                            string sConnection = uri.URIDataManager.DefaultConnection.Replace(
                                Helpers.GeneralHelpers.STRING_DELIMITER, Helpers.GeneralHelpers.FORMELEMENT_DELIMITER2);
                            oPaths.Append(sConnection);
                            oPaths.Append(Helpers.GeneralHelpers.STRING_DELIMITER);
                            //azure blob storage (paramdelimiter is safe for 2.0.0 connection strings)
                            sConnection = uri.URIDataManager.StorageConnection.Replace(
                                Helpers.GeneralHelpers.STRING_DELIMITER, Helpers.GeneralHelpers.FORMELEMENT_DELIMITER2);
                            oPaths.Append(sConnection);
                            i += 1;
                        }
                        sResourcePath = string.Empty;
                        sFullResourcePath = string.Empty;
                        sResourceAltDescription = string.Empty;
                        sResourceURIPattern = string.Empty;
                    }
                }
            }
            sResourceFilePaths = oPaths.ToString();
            return sResourceFilePaths;
        }
        public async Task AddResourceURLsToDeleteCollectionAsync(ContentURI uri,
            EditHelpers.EditHelper.ArgumentsEdits argumentsEdits)
        {
            if (argumentsEdits.SelectionsToAdd != null)
            {
                string sResourceURLs = string.Empty;
                foreach (ContentURI resourceToDeleteURI in argumentsEdits.SelectionsToAdd)
                {
                    AddParentURIPropertiesToResourceURI(uri, resourceToDeleteURI);
                    sResourceURLs = await GetResourceUrlArrayAsync(resourceToDeleteURI);
                    //use the file system string to hold the delimited string
                    resourceToDeleteURI.URIDataManager.FileSystemPath
                        = sResourceURLs;
                }
            }
        }
        //2.0.0 change
        //moved from stylesheethelper to compile
        public async Task<string> GetResourceUrlArrayAsync(ContentURI resourceURI)
        {
            string sResourceURLArray = string.Empty;
            IContentRepositoryEF contentRepository
                = new DevTreks.Data.SqlRepositories.ContentRepository(resourceURI);
            //resourcepath;resourcealt;resourceuri
            if (resourceURI.URIId != 0
                && resourceURI.URINodeName != GeneralHelpers.NONE)
            {
                bool bNeedsFullPath = true;
                bool bNeedsOneRecord = false;
                sResourceURLArray = await contentRepository.GetResourceURLsAsync(resourceURI, bNeedsOneRecord, bNeedsFullPath,
                    Resources.RESOURCES_GETBY_TYPES.storyuri,
                    string.Empty);
                contentRepository.Dispose();
            }
            return sResourceURLArray;
        }
        public static bool DeleteResource(ContentURI uriInit,
           FileStorageIO.PLATFORM_TYPES platformType, EditHelpers.EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bHasDeleted = false;
            if (argumentsEdits.SelectionsToAdd != null)
            {
                string sResourceURLs = string.Empty;
                foreach (ContentURI resourceToDeleteURI in argumentsEdits.SelectionsToAdd)
                {
                    //use the file system string to hold the delimited string
                    sResourceURLs = resourceToDeleteURI.URIDataManager.FileSystemPath;
                    if (!string.IsNullOrEmpty(sResourceURLs))
                    {
                        bHasDeleted = DeleteResource(resourceToDeleteURI, platformType, sResourceURLs);
                    }
                }
            }
            return bHasDeleted;
        }
        public static bool DeleteResource(ContentURI uriInit, 
            FileStorageIO.PLATFORM_TYPES platformType, string resourceURLs)
        {
            bool bHasDeleted = false;
            if (!string.IsNullOrEmpty(resourceURLs))
            {
                string[] arrResourceURLs = 
                    resourceURLs.Split(Helpers.GeneralHelpers.PARAMETER_DELIMITERS);
                if (arrResourceURLs != null)
                {
                    if (arrResourceURLs.Length > 0)
                    {
                        string sResourceURL = string.Empty;
                        string sResourceURIPattern = string.Empty;
                        string sResourceParams = string.Empty;
                        string[] arrResourceParams = {};
                        int i = 0;
                        for (i = 0; i < arrResourceURLs.Length; i++)
                        {
                            sResourceParams = arrResourceURLs[i];
                            if (!string.IsNullOrEmpty(sResourceParams))
                            {
                                arrResourceParams = sResourceParams.Split(Helpers.GeneralHelpers.STRING_DELIMITERS);
                                if (arrResourceParams != null)
                                {
                                    if (arrResourceParams.Length > 0)
                                    {
                                        sResourceURL = arrResourceParams[0];
                                        if (arrResourceParams.Count() > 1)
                                        {
                                            sResourceURIPattern = arrResourceParams[2];
                                            if (!string.IsNullOrEmpty(sResourceURL)
                                                && (!string.IsNullOrEmpty(sResourceURIPattern)))
                                            {
                                                ContentURI resourceURI
                                                    = ContentURI.ConvertShortURIPattern(sResourceURIPattern, uriInit);
                                                AddParentURIPropertiesToResourceURI(uriInit, resourceURI);
                                                if (platformType == FileStorageIO.PLATFORM_TYPES.webserver)
                                                {
                                                    bHasDeleted = DeleteResourceURL(resourceURI, sResourceURL);
                                                }
                                                else if (platformType == FileStorageIO.PLATFORM_TYPES.azure)
                                                {
                                                    bHasDeleted = FileStorageIO.DeleteURI(resourceURI,
                                                        sResourceURL);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return bHasDeleted;
        }
        
        public static bool DeleteResourceURL(ContentURI resourceURI,
            string resourceURIPath)
        {
            bool bIsDeleted = false;
            string sResourceWebPath = string.Empty;
            string sPathToResource = AppSettings.GetResourceRootPath(resourceURI, false);
            int iFullPathIndex
                = resourceURIPath.IndexOf(sPathToResource);
            if (iFullPathIndex < 0)
            {
                sResourceWebPath = string.Concat(sPathToResource,
                     resourceURIPath.Replace(Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER, Helpers.GeneralHelpers.FILE_PATH_DELIMITER));
            }
            else
            {
                sResourceWebPath = resourceURIPath;
            }
            if (Helpers.FileStorageIO.URIAbsoluteExists(resourceURI, sResourceWebPath))
            {
                bIsDeleted = FileStorageIO.DeleteURI(resourceURI, sResourceWebPath);
                //if is the last resource, also delete the directory
                bool bDeleteSubDirs = true;
                FileStorageIO.DeleteDirectory(resourceURI, sResourceWebPath, bDeleteSubDirs);
            }
            return bIsDeleted;
        }
       
        public static void AddParentURIPropertiesToResourceURI(ContentURI parentURI,
            ContentURI resourceURI)
        {
            resourceURI.URIDataManager = new ContentURI.DataManager(parentURI.URIDataManager);
            //resourceURI.URIDataManager.AppType = parentURI.URIDataManager.AppType;
            //resourceURI.URIDataManager.SubAppType = parentURI.URIDataManager.SubAppType;
            //resourceURI.URIDataManager.ServerActionType = parentURI.URIDataManager.ServerActionType;
            //resourceURI.URIDataManager.ServerSubActionType = parentURI.URIDataManager.ServerSubActionType;
            //resourceURI.URIDataManager.DefaultWebDomain = parentURI.URIDataManager.DefaultWebDomain;
            //resourceURI.URIDataManager.DefaultRootFullFilePath = parentURI.URIDataManager.DefaultRootFullFilePath;
            //resourceURI.URIDataManager.DefaultRootWebStoragePath = parentURI.URIDataManager.DefaultRootWebStoragePath;
            if (parentURI.URINetwork != null)
            {
                resourceURI.URINetwork = new Network(parentURI.URINetwork);
            }
        }
        public static string GetResourceArray(int positionInArray, 
            string resourcePath, string altDesc, string resourceURIPattern)
        {
            string sResourceArray = string.Empty;
            StringBuilder arrResource = new StringBuilder();
            if (string.IsNullOrEmpty(resourcePath) == false)
            {
                if (positionInArray == 0)
                {
                    arrResource.Append(resourcePath);
                }
                else
                {
                    arrResource.Append(Helpers.GeneralHelpers.PARAMETER_DELIMITER);
                    arrResource.Append(resourcePath);
                }
                //alt attribute (for accessability)
                arrResource.Append(Helpers.GeneralHelpers.STRING_DELIMITER);
                arrResource.Append(altDesc);
                //name attribute (for caching)
                arrResource.Append(Helpers.GeneralHelpers.STRING_DELIMITER);
                arrResource.Append(resourceURIPattern);
                sResourceArray = arrResource.ToString();
            }
            return sResourceArray;
        }
        public static void GetResourceIdsForResourceFilePaths(string resourceFilePaths,
            int positionInArray, out string relativeResourcePath, out string altDesc,
            out string resourceURIPattern)
        {
            relativeResourcePath = string.Empty;
            altDesc = string.Empty;
            resourceURIPattern = string.Empty;
            if (resourceFilePaths != string.Empty)
            {
                string[] arrResourcePaths = resourceFilePaths.Split(Helpers.GeneralHelpers.PARAMETER_DELIMITERS);
                if (arrResourcePaths != null)
                {
                    if (arrResourcePaths.Length >= (positionInArray + 1))
                    {
                        string sResourceIds = arrResourcePaths[positionInArray];
                        string[] arrResourceIds = sResourceIds.Split(Helpers.GeneralHelpers.STRING_DELIMITERS);
                        if (arrResourceIds != null)
                        {
                            if (arrResourceIds.Length >= 3)
                            {
                                relativeResourcePath = arrResourceIds[0];
                                altDesc = arrResourceIds[1];
                                resourceURIPattern = arrResourceIds[2];
                            }
                        }
                    }
                }
            }
        }
        public async Task<List<ContentURI>> FillResourceListAsync(ContentURI uri,
            SqlDataReader resourceResults)
        {
            List<ContentURI> colResource = new List<ContentURI>();
            if (resourceResults != null)
            {
                string sFileExtensionType = string.Empty;
                while (await resourceResults.ReadAsync())
                {
                    //use the resourceuripattern to init a ContentURI
                    ContentURI resource = new
                        ContentURI(resourceResults.GetString(2));
                    //this is a std rel file path to a resource
                    string sRelFilePath = resourceResults.GetString(0);
                    resource.URIDataManager.FileSystemPath = sRelFilePath;
                    Helpers.FileStorageIO.PLATFORM_TYPES ePlatform
                        = Helpers.FileStorageIO.GetPlatformType(uri);
                    if (ePlatform == Helpers.FileStorageIO.PLATFORM_TYPES.azure)
                    {
                        //the path has to be to the blob
                        if (!string.IsNullOrEmpty(sRelFilePath))
                        {
                            AzureIOAsync azureIO = new AzureIOAsync(uri);
                            CloudBlockBlob blob = azureIO.GetBlob(sRelFilePath);
                            resource.URIDataManager.FileSystemPath = blob.Uri.AbsoluteUri;
                        }
                    }
                    //this is a short description for an image's alt attribute
                    resource.URIDataManager.Description = resourceResults.GetString(1);
                    colResource.Add(resource);
                }
            }
            return colResource;
        }
        public string GetAndSaveResourceURLInFilesByResourcePackId( 
            ContentURI uri, string resourcePackId)
        {
            //this method is called by resource nodes
            string sResourceFilePath = string.Empty;
            //use a full file system path for storing it
            bool bIsFileSystemPath = true;
            sResourceFilePath = GetResourceFilePath(uri, bIsFileSystemPath,
                uri.URIDataManager.SubAppType.ToString(), uri.URINetworkPartName, resourcePackId,
                uri.URIId.ToString(), uri.URIName);
            //prefer to use async and owners only but
            //had to use this because localhost moves around and owner may not be able to set all resources
            if ((Helpers.FileStorageIO.URIAbsoluteExists(uri,
                sResourceFilePath) == false
                || (Path.HasExtension(sResourceFilePath) == false))
                && sResourceFilePath != string.Empty)
            {
                //ContentViewModel.GetResourceAsync handles all resource state management
                //if this condition is true that methods is not working
                //Task resourcesLVS = StoreResourceInFileSystemByIdAsync(sqlIO, sResourceFilePath, uri);
            }
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri,
                sResourceFilePath) == true)
            {
                //use a full web path for web pages
                bIsFileSystemPath = false;
                sResourceFilePath = GetResourceFilePath(uri, bIsFileSystemPath,
                    uri.URIDataManager.SubAppType.ToString(), uri.URINetworkPartName, resourcePackId,
                    uri.URIId.ToString(), uri.URIName);
            }
            else
            {
                sResourceFilePath = string.Empty;
            }
            return sResourceFilePath;
        }
        public async Task<string> GetAndSaveResourceURLInFilesByResourcePackIdAsync(
            ContentURI uri, string resourcePackId)
        {
            //this method is called by resource nodes
            string sResourceFilePath = string.Empty;
            //use a full file system path for storing it
            bool bIsFileSystemPath = true;
            sResourceFilePath = GetResourceFilePath(uri, bIsFileSystemPath,
                uri.URIDataManager.SubAppType.ToString(), uri.URINetworkPartName, resourcePackId, 
                uri.URIId.ToString(), uri.URIName);
            if ((Helpers.FileStorageIO.URIAbsoluteExists(uri,
                sResourceFilePath) == false
                || (Path.HasExtension(sResourceFilePath) == false))
                && sResourceFilePath != string.Empty)
            {
                await StoreResourceInFileSystemByIdAsync(sResourceFilePath, uri);
            }
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri, sResourceFilePath) == true)
            {
                //use a full web path for web pages
                bIsFileSystemPath = false;
                sResourceFilePath = GetResourceFilePath(uri, bIsFileSystemPath,
                    uri.URIDataManager.SubAppType.ToString(), uri.URINetworkPartName, resourcePackId,
                    uri.URIId.ToString(), uri.URIName);
            }
            else
            {
                sResourceFilePath = string.Empty;
            }
            return sResourceFilePath;
        }
        public string GetAndSaveResourceURLInCloudByResourcePackId(
            ContentURI uri, string resourcePackId)
        {
            //this method is called by resource nodes
            string sResourceURIPath = string.Empty;
            //the blob path will be the blob's name
            bool bIsFileSystemPath = false;
            string sResourceRelPath = GetResourceFilePath(uri, bIsFileSystemPath,
                uri.URIDataManager.SubAppType.ToString(), uri.URINetworkPartName, resourcePackId,
                uri.URIId.ToString(), uri.URIName);
            AzureIOAsync azureIO = new AzureIOAsync(uri);
            sResourceURIPath = azureIO.GetAndSaveResourceURLInCloud(uri, sResourceRelPath);
            return sResourceURIPath;
        }

        public async Task StoreResourceInFileSystemByIdAsync( 
            string resourceFilePath, ContentURI uri)
        {
            bool bIsXmlDoc = Helpers.GeneralHelpers.IsXmlFileExt(uri.URIName);
            if (!bIsXmlDoc)
                bIsXmlDoc = Helpers.GeneralHelpers.IsXmlFileExt(resourceFilePath);
            if (uri.URINodeName == Resources.RESOURCES_TYPES.resourcepack.ToString())
            {
                bIsXmlDoc = true;
            }
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] oPrams =
		    {
			    sqlIO.MakeInParam("@PKId",              SqlDbType.Int, 4, uri.URIId),
                sqlIO.MakeInParam("@IsXmlDoc",          SqlDbType.Bit, 1, bIsXmlDoc)
		    };
            // uses top 1 resource 
            //large data is retrieved sequentially (rather than all into memory at once)
            SqlDataReader oDataReader = await sqlIO.RunSequentialProcAsync("0GetResourceByResourceId",
                oPrams);
            if (oDataReader != null && (!oDataReader.IsClosed))
            {
                bool bIsFileSystemPath = true;
                if (bIsXmlDoc)
                {
                    SaveXmlInFileSystem(uri, oDataReader, resourceFilePath);
                }
                else
                {
                    SaveBinaryInFileSystem(uri, bIsFileSystemPath, oDataReader, resourceFilePath);
                }
            }
            sqlIO.Dispose();
        }
        
        private void SaveXmlInFileSystem(ContentURI uri, 
            SqlDataReader dataReader, string resourceFilePath)
        {
            using (dataReader)
            {
                while (dataReader.Read())
                {
                    //update the filename
                    if (dataReader.IsDBNull(1) == false)
                    {
                        string sFileName = dataReader.GetString(1);
                        if (!string.IsNullOrEmpty(sFileName))
                        {
                            resourceFilePath = Helpers.GeneralHelpers.ChangeFileNameInPath(resourceFilePath, sFileName);
                        }
                    }
                    if (dataReader.IsDBNull(2) == false)
                    {
                        SqlXml oSqlXml = dataReader.GetSqlXml(2);
                        if (oSqlXml.IsNull == false)
                        {
                            XmlReader oReader = oSqlXml.CreateReader();
                            Helpers.XmlFileIO oXmlFileIO = new Helpers.XmlFileIO();
                            using (oReader)
                            {
                                oXmlFileIO.WriteFileXml(uri, oReader, resourceFilePath);
                            }
                        }
                    }
                }
            }
        }
        private void SaveBinaryInFileSystem(ContentURI uri, bool isFileSystemPath,
            SqlDataReader dataReader, string resourceFilePath)
        {
            if (dataReader != null && (!dataReader.IsClosed))
            {
                using (dataReader)
                {
                    while (dataReader.Read())
                    {
                        string sResourceMimeType = string.Empty;
                        //sequential data has to be read in order (once an index is passed, can't return to it)
                        //get the media type
                        if (dataReader.IsDBNull(0) == false)
                        {
                            sResourceMimeType = dataReader.GetString(0);
                        }
                        //update the filename
                        if (dataReader.IsDBNull(1) == false)
                        {
                            string sFileName = dataReader.GetString(1);
                            if (!string.IsNullOrEmpty(sFileName))
                            {
                                resourceFilePath = Helpers.GeneralHelpers.ChangeFileNameInPath(resourceFilePath, sFileName);
                            }
                        }
                        if (dataReader.IsDBNull(2) == false)
                        {
                            if (Helpers.FileStorageIO.DirectoryCreate(uri,
                                resourceFilePath))
                            {
                                if (sResourceMimeType.StartsWith("image"))
                                {
                                    //Images should be cached in client browser, using Apps.General.DisplayXhtml.WriteResourceURLsForClientCaching)
                                    SaveBinaryFile(dataReader, 2, resourceFilePath);
                                }
                                else
                                {
                                    //just save the bytes as binary
                                    SaveBinaryFile(dataReader, 2, resourceFilePath);
                                }
                            }
                        }
                    }
                }
            }
        }
        
        
        
        private void SaveBinaryFile(SqlDataReader sqlReader, int colIndex,
            string resourceFilePath)
        {
            //184: new because GetBytes was not getting the final byte
            FileIO.CopyBinaryValueToFile(resourceFilePath, sqlReader, colIndex);
            //Helpers.FileIO oFileIO = new Helpers.FileIO();
            //oFileIO.WriteBinaryBlobFile(resourceFilePath, sqlReader, colIndex);
        }
       
        public async Task<bool> SaveURIResourceFileAsync(ContentURI uri,
            string fileName, int fileLength, string mimeType,
            Stream postedFileStream)
        {
            bool bHasUpdated = false;
            int iSPReturn = 0;
            using (postedFileStream)
            {
                string sQry = string.Empty;
                bool bIsXmlMimeType = Helpers.GeneralHelpers.IsXmlMimeType(mimeType);
                if (uri.URINodeName == RESOURCES_TYPES.resourcepack.ToString())
                {
                    //double insurance
                    bIsXmlMimeType = true;
                    uri.URIDataManager.AttributeName = AppHelpers.General.RPATTNAME;
                }
                //less expensive to store blobs in file and blob system rather than db
                bool bCanBeStoredInDb 
                    = RuleHelpers.ResourceRules.VerifyFileLengthForDBStorage(uri, fileLength);
                if (bCanBeStoredInDb)
                {
                    //convert the stream and store in the database and change the last modified date
                    //store xml data types separate from varbinary datatypes (they are not fully compatible)
                    //varbinary fails if byte length is greater than 4mb or 8000
                    byte[] arrFileBytes = new byte[fileLength];
                    if (bIsXmlMimeType)
                    {
                        XmlReader oXmlReader = XmlReader.Create(postedFileStream);
                        if (oXmlReader != null)
                        {
                            using (oXmlReader)
                            {
                                SqlXml sqlXmlValue = new SqlXml(oXmlReader);
                                Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
                                SqlParameter[] oPrams =
		                        {
			                        sqlIO.MakeInParam("@PKId",					SqlDbType.Int, 4, uri.URIId),
                                    sqlIO.MakeInParam("@Name",					SqlDbType.NVarChar, 75, fileName),
                                    sqlIO.MakeInParam("@MimeType",			    SqlDbType.NVarChar, 75, mimeType),
			                        sqlIO.MakeInParam("@LastChangedDate",	    SqlDbType.SmallDateTime, 8,  Helpers.GeneralHelpers.GetDateShortNow()),
                                    sqlIO.MakeInParam("@AttName",			    SqlDbType.NVarChar, 25, uri.URIDataManager.AttributeName),
                                    sqlIO.MakeInParam("@Xml",			        SqlDbType.Xml, 0, sqlXmlValue)
		                        };
                                sQry = "0UpdateResourceXml";
                                iSPReturn = await sqlIO.RunProcIntAsync(sQry, oPrams);
                                sqlIO.Dispose();
                            }
                        }
                    }
                    else
                    {
                        //read the stream into the byte array
                        postedFileStream.Read(arrFileBytes, 0, fileLength);
                        Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
                        SqlParameter[] oPrams =
		                {
			                sqlIO.MakeInParam("@PKId",					SqlDbType.Int, 4,  uri.URIId),
                            sqlIO.MakeInParam("@Name",					SqlDbType.NVarChar, 75, fileName),
                            sqlIO.MakeInParam("@MimeType",			    SqlDbType.NVarChar, 75, mimeType),
			                sqlIO.MakeInParam("@LastChangedDate",	    SqlDbType.SmallDateTime, 8,  Helpers.GeneralHelpers.GetDateShortNow()),
                            sqlIO.MakeInParam("@Binary",			    SqlDbType.VarBinary, fileLength, arrFileBytes)
		                };
                        sQry = "0UpdateResourceBinary";
                        iSPReturn = await sqlIO.RunProcIntAsync(sQry, oPrams);
                        sqlIO.Dispose();
                    }
                    bHasUpdated = true;
                    //resourcepacks are displayed in razor pages (not through urls)
                    if (uri.URINodeName != Resources.RESOURCES_TYPES.resourcepack.ToString())
                    {
                        await GetPathAndSaveResourceAsync(uri, fileName);
                    }
                }
                else
                {
                    //resourcepacks should not be this large
                    if (uri.URINodeName != Resources.RESOURCES_TYPES.resourcepack.ToString()
                        && RuleHelpers.ResourceRules.VerifyFileLength(uri, fileLength))
                    {
                        //save the xml or binary in filesystem or blobsystem
                        bool bIsSaved = await GetPathAndSaveResourceAsync(uri, mimeType, fileName, postedFileStream);
                        if (bIsSaved)
                        {
                            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
                            //update the resource fields but not the xml or binary fields
                            SqlParameter[] oPrams =
		                    {
			                    sqlIO.MakeInParam("@PKId",					SqlDbType.Int, 4, uri.URIId),
                                sqlIO.MakeInParam("@Name",					SqlDbType.NVarChar, 75, fileName),
                                sqlIO.MakeInParam("@MimeType",			    SqlDbType.NVarChar, 75, mimeType),
			                    sqlIO.MakeInParam("@LastChangedDate",	    SqlDbType.SmallDateTime, 8,  Helpers.GeneralHelpers.GetDateShortNow()),
                                sqlIO.MakeInParam("@AttName",			    SqlDbType.NVarChar, 25, uri.URIDataManager.AttributeName)
		                    };
                            sQry = "0UpdateResource";
                            iSPReturn = await sqlIO.RunProcIntAsync(sQry, oPrams);
                            sqlIO.Dispose();
                            bHasUpdated = true;
                        }
                    }
                }
            }
            return bHasUpdated;
        }
        public async Task GetPathAndSaveResourceAsync(
            ContentURI resourceURI, string newFileName)
        {
            //set the resource path and saves the resource in storage
            bool bIsFileSystemPath = FileStorageIO.IsFileSystemFile(resourceURI.URIClub.ClubDocFullPath);
            string sResourcePackId
                = ContentURI.GetURIPatternPart(resourceURI.URIDataManager.ParentURIPattern, ContentURI.URIPATTERNPART.id);
            string sResourcePath = GetResourceFilePath(resourceURI, bIsFileSystemPath,
                resourceURI.URIDataManager.SubAppType.ToString(), resourceURI.URINetworkPartName, sResourcePackId,
                resourceURI.URIId.ToString(), resourceURI.URIName);
            //delete old resource and update name
            sResourcePath = UpdateOldResource(resourceURI, sResourcePath, newFileName);
            bool bNeedsFullPath = true;
            await GetPathAndSaveResourceAsync(resourceURI, sResourcePath, bNeedsFullPath);
        }
        public async Task<bool> GetPathAndSaveResourceAsync(ContentURI resourceURI, 
            string mimeType, string newFileName, Stream postedFileStream)
        {
            bool bIsSaved = false;
            //set the resource path and saves the resource in storage
            bool bIsFileSystemPath = FileStorageIO.IsFileSystemFile(resourceURI.URIClub.ClubDocFullPath);
            string sResourcePackId
                = ContentURI.GetURIPatternPart(resourceURI.URIDataManager.ParentURIPattern, ContentURI.URIPATTERNPART.id);
            string sResourcePath = GetResourceFilePath(resourceURI, bIsFileSystemPath,
                resourceURI.URIDataManager.SubAppType.ToString(), resourceURI.URINetworkPartName, sResourcePackId,
                resourceURI.URIId.ToString(), resourceURI.URIName);
            //delete old resource and update name
            sResourcePath = UpdateOldResource(resourceURI, sResourcePath, newFileName);
            bool bNeedsFullPath = true;
            bIsSaved = await GetPathAndSaveResourceAsync(resourceURI, sResourcePath, bNeedsFullPath, 
                mimeType, postedFileStream);
            return bIsSaved;
        }
        private string UpdateOldResource(ContentURI resourceURI, string resourcePath, string newFileName)
        {
            //delete any existing uri (delete the directory in case the filename has changed)
            bool bIncludeSubDirs = true;
            FileStorageIO.DeleteAndReplaceDirectory(resourceURI, resourcePath, bIncludeSubDirs);
            //change to the the potentially new filename
            string sResourcePath = resourcePath.Replace(Path.GetFileName(resourcePath), newFileName);
            return sResourcePath;
        }
        public async Task<bool> GetPathAndSaveResourceAsync(ContentURI resourceURI, string resourcePath,
            bool needFullPath, string mimeType, Stream postedFileStream)
        {
            bool bIsSaved = false;
            bool bIsXmlMimeType = Helpers.GeneralHelpers.IsXmlMimeType(mimeType);
            string sErrorMsg = string.Empty;
            if (bIsXmlMimeType)
            {
                XmlReader oXmlReader = XmlReader.Create(postedFileStream);
                if (oXmlReader != null)
                {
                    FileStorageIO fs = new FileStorageIO();
                    sErrorMsg = await fs.SaveXmlInURIAsync(resourceURI, oXmlReader,
                        resourcePath);
                    resourceURI.ErrorMessage = sErrorMsg;
                }
            }
            else
            {
                FileStorageIO fs = new FileStorageIO();
                sErrorMsg = await fs.SaveBinaryStreamInURIAsync(resourceURI, postedFileStream, resourcePath);
                resourceURI.ErrorMessage = sErrorMsg;
            }
            if (string.IsNullOrEmpty(sErrorMsg))
            {
                bIsSaved = true;
            }
            return bIsSaved;
        }
        //storage by owner
        public async Task GetPathAndSaveResourceAsync( 
            ContentURI resourceURI, string resourcePath,
             bool needFullPath)
        {
            Helpers.FileStorageIO.PLATFORM_TYPES ePlatform
                = Helpers.FileStorageIO.GetPlatformType(resourceURI);
            if (ePlatform == Helpers.FileStorageIO.PLATFORM_TYPES.azure)
            {
                Helpers.AzureIOAsync azuriIO = new AzureIOAsync(resourceURI);
                resourceURI.URIDataManager.FileSystemPath =
                    await azuriIO.SaveResourceURLInCloudAsync(resourceURI,
                        resourcePath);
            }
            else
            {
                resourceURI.URIDataManager.FileSystemPath =
                    await GetAndSaveResourceURLInFilesAsync(resourceURI,
                        resourcePath, needFullPath);
            }
        }
        //retrieval by client
        public void GetPathAndSaveResource( 
            ContentURI resourceURI, string resourcePath,
             bool needFullPath)
        {
            Helpers.FileStorageIO.PLATFORM_TYPES ePlatform
                = Helpers.FileStorageIO.GetPlatformType(resourceURI);
            if (ePlatform == Helpers.FileStorageIO.PLATFORM_TYPES.azure)
            {
                Helpers.AzureIOAsync azuriIO = new AzureIOAsync(resourceURI);
                resourceURI.URIDataManager.FileSystemPath =
                    azuriIO.SaveResourceURLInCloud(resourceURI,
                        resourcePath);
            }
            else
            {
                resourceURI.URIDataManager.FileSystemPath =
                    GetResourceURL(resourceURI,
                        resourcePath, needFullPath);
            }
        }
        public async Task<bool> GetPathAndStoreResourceAsync(ContentURI resourceURI)
        {
            bool bIsSet = false;
            //uses absolute file system paths
            string sFinalResourcePath = string.Empty;
            //set the resource path and only stores the resource if it isn't stored yet
            bool bIsFileSystemPath = false;
            if (resourceURI.URIClub != null)
            {
                bIsFileSystemPath = FileStorageIO.IsFileSystemFile(resourceURI.URIClub.ClubDocFullPath);
            }
            string sResourcePackId 
                = ContentURI.GetURIPatternPart(resourceURI.URIDataManager.ParentURIPattern, ContentURI.URIPATTERNPART.id);
            if (!string.IsNullOrEmpty(sResourcePackId))
            {
                string sResourcePath = GetResourceFilePath(resourceURI, bIsFileSystemPath,
                    resourceURI.URIDataManager.SubAppType.ToString(), resourceURI.URINetworkPartName, sResourcePackId,
                    resourceURI.URIId.ToString(), resourceURI.URIName);
                if (!string.IsNullOrEmpty(sResourcePath))
                {
                    bool bNeedsFullPath = true;
                    await GetPathAndStoreResourceAsync(resourceURI, sResourcePath, bNeedsFullPath);
                    bIsSet = true;
                }
            }
            return bIsSet;
        }
        public async Task GetPathAndStoreResourceAsync( 
            ContentURI resourceURI, string resourcePath,
            bool needFullPath)
        {
            //resourcePath is not rooted yet (it's just networkcrops/resourcepack_271 ....
            Helpers.FileStorageIO.PLATFORM_TYPES ePlatform
                = Helpers.FileStorageIO.GetPlatformType(resourceURI);
            if (ePlatform == Helpers.FileStorageIO.PLATFORM_TYPES.azure)
            {
                Helpers.AzureIOAsync azuriIO = new AzureIOAsync(resourceURI);
                resourceURI.URIDataManager.FileSystemPath =
                    await azuriIO.GetAndSaveResourceURLInCloudAsync(resourceURI,
                        resourcePath);
            }
            else
            {
                resourceURI.URIDataManager.FileSystemPath =
                    await GetAndSaveResourceURLInFilesAsync(resourceURI,
                        resourcePath, needFullPath);
            }
        }
        public void GetPathAndStoreResource( 
            ContentURI resourceURI, string resourcePath,
            bool needFullPath)
        {
            //resourcePath is not rooted yet (it's just networkcrops/resourcepack_271 ....
            Helpers.FileStorageIO.PLATFORM_TYPES ePlatform
                = Helpers.FileStorageIO.GetPlatformType(resourceURI);
            if (ePlatform == Helpers.FileStorageIO.PLATFORM_TYPES.azure)
            {
                Helpers.AzureIOAsync azuriIO = new AzureIOAsync(resourceURI);
                resourceURI.URIDataManager.FileSystemPath =
                    azuriIO.GetAndSaveResourceURLInCloud(resourceURI,
                        resourcePath);
            }
            else
            {
                resourceURI.URIDataManager.FileSystemPath =
                    GetAndSaveResourceURLInFiles(resourceURI,
                        resourcePath, needFullPath);
            }
        }
        public async Task<bool> UpdateURIResourceFileUploadMsgAsync( 
            ContentURI fileUploadURI, string message)
        {
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(fileUploadURI);
            SqlParameter[] oPrams =
		    {
			    sqlIO.MakeInParam("@PKId",              SqlDbType.Int, 4, fileUploadURI.URIId),
                sqlIO.MakeInParam("@ErrorMsg",          SqlDbType.NVarChar, 150, message)
		    };
            //adds an error message to end of long description (feedback about file upload)
            int iReturned = await sqlIO.RunProcIntAsync( 
                "0UpdateResourceDescription", oPrams);
            bool bIsUpdated = true;
            sqlIO.Dispose();
            return bIsUpdated;
        }
        public static void GetChildForeignKeyName(string parentNodeName,
            out string childForeignKeyName)
        {
            childForeignKeyName = string.Empty;
            if (parentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                childForeignKeyName = Agreement.SERVICE_ID;
            }
            else if (parentNodeName == RESOURCES_TYPES.resourcegroup.ToString())
            {
                childForeignKeyName = "ResourceGroupId";
            }
            else if (parentNodeName == RESOURCES_TYPES.resourcepack.ToString())
            {
                childForeignKeyName = "ResourcePackId";
            }
            else if (parentNodeName == RESOURCES_TYPES.resource.ToString())
            {
                //no children
            }
        }
        public static ContentURI GetResourceURIFromResourcePath(ContentURI uri, string resourcePath)
        {
            string sResourceURIPattern
                    = AppHelpers.Resources.GetURIPatternFromResourcePath(resourcePath);
            ContentURI oResourceURI = ContentURI.ConvertShortURIPattern(sResourceURIPattern, uri);
            return oResourceURI;
        }
        public static void SetURIandParentURIPatternFromResourcePath(string resourcePath, 
            ContentURI resourceURI)
        {
            string sResourceURIPattern = string.Empty;
            //network_cattle/resourcepack_155/resource_1234/blue.gif
            string sNetworkName = string.Empty;
            string sResourcePackId = string.Empty;
            string sResourceId = string.Empty;
            string sResourceFileName = string.Empty;
            GetStandardIdsFromResourcePath(resourcePath,
                out sNetworkName, out sResourcePackId, out sResourceId,
                out sResourceFileName);
            if (!string.IsNullOrEmpty(sResourcePackId)
                && !string.IsNullOrEmpty(sResourceId)
                && !string.IsNullOrEmpty(sNetworkName))
            {
                //need the actual file name that will be stored so don't change
                sResourceURIPattern = Helpers.GeneralHelpers.MakeURIPattern(sResourceFileName,
                    sResourceId, sNetworkName, RESOURCES_TYPES.resource.ToString(),
                    string.Empty);
                resourceURI.ChangeURIPatternNoDbHit(sResourceURIPattern);
                string sParentURIPattern = Helpers.GeneralHelpers.MakeURIPattern(sResourceFileName,
                    sResourcePackId, sNetworkName, RESOURCES_TYPES.resourcepack.ToString(),
                    string.Empty);
                //use the resourcepackid foundin relativepath to make a parenturi
                resourceURI.URIDataManager.ParentURIPattern = sParentURIPattern;
            }
        }
        public static string GetURIPatternFromResourcePath(string resourcePath)
        {
            string sResourceURIPattern = string.Empty;
            //network_cattle/resourcepack_155/resource_1234/blue.gif
            string sNetworkName = string.Empty;
            string sResourcePackId = string.Empty;
            string sResourceId = string.Empty;
            string sResourceFileName = string.Empty;
            GetStandardIdsFromResourcePath(resourcePath,
                out sNetworkName, out sResourcePackId, out sResourceId,
                out sResourceFileName);
            if (!string.IsNullOrEmpty(sResourcePackId) 
                && !string.IsNullOrEmpty(sResourceId)
                && !string.IsNullOrEmpty(sNetworkName))
            {
                sResourceFileName = sResourceFileName.Replace(
                    Helpers.GeneralHelpers.FILEEXTENSION_DELIMITER, "-");
                sResourceURIPattern = Helpers.GeneralHelpers.MakeURIPattern(sResourceFileName,
                    sResourceId, sNetworkName, RESOURCES_TYPES.resource.ToString(),
                    string.Empty);
            }
            return sResourceURIPattern;
        }
        public static void GetStandardIdsFromResourcePath(string resourcePath, 
            out string networkName, out string resourcePackId, out string resourceId, 
            out string resourceFileName)
        {
            networkName = string.Empty;
            resourcePackId = string.Empty;
            resourceId = string.Empty;
            resourceFileName = string.Empty;
            string[] arrResourcePathParts = null;
            int iIndex 
                = resourcePath.IndexOf(Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER);
            if (iIndex < 0)
            {
                arrResourcePathParts = resourcePath.Split(
                    Helpers.GeneralHelpers.FILE_PATH_DELIMITERS);
            }
            else
            {
                arrResourcePathParts = resourcePath.Split(
                    Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITERS);
            }
            if (arrResourcePathParts != null)
            {
                int iArrayLength = arrResourcePathParts.Length;
                if (iArrayLength > 0)
                {
                    int i = 0;
                    string sURIPatternPart = string.Empty;
                    for (i = 0; i < iArrayLength; i++)
                    {
                        sURIPatternPart = string.Empty;
                        if (i == (iArrayLength - 4))
                        {
                            sURIPatternPart
                                = Helpers.GeneralHelpers.GetLastSubString(arrResourcePathParts[i],
                                    Helpers.GeneralHelpers.FILENAME_DELIMITER);
                            networkName = sURIPatternPart;
                        }
                        else if (i == (iArrayLength - 3))
                        {
                            sURIPatternPart
                               = Helpers.GeneralHelpers.GetLastSubString(arrResourcePathParts[i],
                                   Helpers.GeneralHelpers.FILENAME_DELIMITER);
                            resourcePackId = sURIPatternPart;
                        }
                        else if (i == (iArrayLength - 2))
                        {
                            sURIPatternPart
                                = Helpers.GeneralHelpers.GetLastSubString(arrResourcePathParts[i],
                                Helpers.GeneralHelpers.FILENAME_DELIMITER);
                            resourceId = sURIPatternPart;
                        }
                        else if (i == (iArrayLength - 1))
                        {

                            resourceFileName = arrResourcePathParts[i];
                        }
                    }
                }
            }
        }
        public static void SetResourceDefaults(ContentURI uri,
            ContentURI resourceURI, string resourceExtensionObjectNameSpace)
        {
            bool bIsDefaultImage = IsResourceImage(resourceURI);
            if (bIsDefaultImage)
            {
                if (uri.URIDataManager.Resource != null)
                {
                    foreach (ContentURI imageURI in uri.URIDataManager.Resource)
                    {
                        imageURI.URIDataManager.IsMainImage = false;
                    }
                }
            }
            resourceURI.URIDataManager.IsMainImage = bIsDefaultImage;
            bool bIsMainStylesheet = 
                (resourceURI.URIDataManager.FileSystemPath.EndsWith(".xslt")) 
                ? true : false;
            if (bIsMainStylesheet)
            {
                if (uri.URIDataManager.Resource != null)
                {
                    foreach (ContentURI stylesheetURI in uri.URIDataManager.Resource)
                    {
                        stylesheetURI.URIDataManager.IsMainStylesheet = false;
                    }
                }
            }
            resourceURI.URIDataManager.IsMainStylesheet = bIsMainStylesheet;
        }
        public static string GetResourceArrayFromResourceArraysByTagName(
            string resourceArrays, string resourceTagName)
        {
            string sResourceArray = string.Empty;
            if (!string.IsNullOrEmpty(resourceArrays))
            {
                string[] arrResource
                    = resourceArrays.Split(Helpers.GeneralHelpers.PARAMETER_DELIMITERS);
                if (arrResource != null)
                {
                    int iLength = arrResource.Length;
                    int i = 0;
                    int iIndex = 0;
                    for (i = 0; i < iLength; i++)
                    {
                        sResourceArray = arrResource[i];
                        //stored procedure add the resourcetagname 
                        //as the uri.urifileextension property in the uripattern
                        iIndex = sResourceArray.IndexOf(resourceTagName);
                        if (iIndex != -1)
                        {
                            return sResourceArray;
                        }
                        else
                        {
                            sResourceArray = string.Empty;
                        }
                    }

                }
            }
            return sResourceArray;
        }
        public static string GetResourceQueryName(string currentNodeName)
        {
            string sQryName = string.Empty;
            if (currentNodeName == RESOURCES_TYPES.resource.ToString())
            {
                sQryName = "0GetResourceLinkedViewXml";
            }
            return sQryName;
        }
        public static string GetUpdateResourceQueryName(ContentURI uri)
        {
            string sQryName = string.Empty;
            if (uri.URINodeName == RESOURCES_TYPES.resource.ToString())
            {
                sQryName = "0UpdateResourceLinkedViewXml";
            }
            return sQryName;
        }
        public static string GetResourceFilePath(ContentURI resourceURI,
            bool isFileSystemPath,
            string serverSubActionType, string networkId,
            string resourcePackId, string resourceId,
            string resourceName)
        {
            string sPathToResource = string.Empty;
            if (string.IsNullOrEmpty(networkId) == false && string.IsNullOrEmpty(resourcePackId) == false
                && string.IsNullOrEmpty(resourceId) == false && string.IsNullOrEmpty(resourceName) == false)
            {
                string sResourceFilePath = string.Empty;
                string sPathDelimiter = string.Empty;
                bool bNeedsWebPath = true;
                if (isFileSystemPath)
                {
                    //i.e. c:\\DevTreks\resources\
                    bNeedsWebPath = false;
                    sPathToResource = AppSettings.GetResourceRootPath(resourceURI, bNeedsWebPath);
                    sPathDelimiter = Helpers.GeneralHelpers.FILE_PATH_DELIMITER;
                }
                else
                {
                    //i.e. www.devtreks.org/content/resources/
                    if (serverSubActionType == Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.makepackage.ToString())
                    {
                        //strive to keep the same web paths for all resources regardless of packages
                        sPathToResource = AppSettings.GetResourceRootPath(resourceURI, bNeedsWebPath);
                    }
                    else
                    {
                        sPathToResource = AppSettings.GetResourceRootPath(resourceURI, bNeedsWebPath);
                    }
                    sPathDelimiter = Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER;
                }
                StringBuilder oPath = new StringBuilder();
                oPath.Append(sPathToResource);
                //each network can have it's own db with own ids
                oPath.Append(AppHelpers.Networks.NETWORK_TYPES.network.ToString());
                oPath.Append(Helpers.GeneralHelpers.FILENAME_DELIMITER);
                oPath.Append(networkId);
                oPath.Append(sPathDelimiter);
                oPath.Append(AppHelpers.Resources.RESOURCES_TYPES.resourcepack.ToString());
                oPath.Append(Helpers.GeneralHelpers.FILENAME_DELIMITER);
                oPath.Append(resourcePackId);
                oPath.Append(sPathDelimiter);
                oPath.Append(AppHelpers.Resources.RESOURCES_TYPES.resource.ToString());
                oPath.Append(Helpers.GeneralHelpers.FILENAME_DELIMITER);
                oPath.Append(resourceId);
                oPath.Append(sPathDelimiter);
                oPath.Append(resourceName);
                sPathToResource = oPath.ToString();
            }
            return sPathToResource;
        }
    }
}
