using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TAFE_ICT40120_Assessment02.Classes;
using TAFE_ICT40120_Assessment02.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace TAFE_ICT40120_Assessment02.Classes
{
    /// <summary>
    /// Create NoticeBoard to show updated information
    /// This class using like Notice or news board
    /// To share the feed information with who using this program
    /// </summary>
    public class NoticeBoard : DummyData
    {
        public string filePathNotice = "..\\..\\..\\Sources\\Notice.txt";
        public List<string> notice;
        public int insertIndex = 6;

        public NoticeBoard()
        {
           
        }

        /// <summary>
        /// Get information strings
        /// First _sort for showing catalogue like Notice or Normal
        /// Second _text for sharing your messages 
        /// </summary>
        /// <param name="_sort"></param>
        /// <param name="_text"></param>
        /// <returns></returns>
        public string AddNotice(string _sort, string _text)
        {
            string notice;
            if (_text == "") return null;
            if (_sort.Equals(MAINBUTTONS.NOTICE.ToString()))
            {
                _sort = $"**** {_sort} ****  ";
                _text = $"{_text.ToUpper()}";
            }
            notice = $"[{DateTime.Now.ToString("dd/MM/yy HH:mm:ss")}]  {_sort}  {_text}\n";
            return notice;
        }

        /// <summary>
        /// When Job added send Notice to inform to share
        /// </summary>
        /// <param name="_job"></param>
        /// <returns></returns>
        public string AddNotice(Job _job)
        {
            return $"[{DateTime.Now.ToString("dd/MM/yy HH:mm:ss")}]  ***************** SK. AGENCY NEWS *****************\n[{DateTime.Now.ToString("dd/MM/yy HH:mm:ss")}]  **** NOTICE ****   NEW JOB ADDED: [ {_job.Title.ToString().ToUpper()} ]\n";
        }

        /// <summary>
        /// When Contractor added send Notice to inform to share
        /// </summary>
        /// <param name="_contractor"></param>
        /// <returns></returns>
        public string AddNotice(Contractor _contractor)
        {
            return $"[{DateTime.Now.ToString("dd/MM/yy HH:mm:ss")}]  **** NOTICE ****   NEW CONTRACTOR ADDED:  [ {_contractor.FullName.ToString().ToUpper()} ]\n";
        }

        /// <summary>
        /// When Job Completed send Notice to inform to share
        /// </summary>
        /// <param name="_completed"></param>
        /// <returns></returns>
        public string AddNotice(Completed _completed)
        {
            return $"[{DateTime.Now.ToString("dd/MM/yy HH:mm:ss")}]  **** NOTICE ****   Job TITLE: [ {_completed.CompletedJob.Title.ToString().ToUpper()} ]\n[{DateTime.Now.ToString("dd/MM/yy HH:mm:ss")}]  **** NOTICE ****   Job COMPLETED BY: [ {_completed.CompletedBy.FullName.ToString().ToUpper()} ]\n";
        }

        /// <summary>
        /// Dummy Notice will added for completed Job to inform to share
        /// </summary>
        /// <param name="_completed"></param>
        /// <returns></returns>
        public string AddDummyNotice(Completed _completed)
        {
            return AddNotice(_completed);
        }

        /// <summary>
        /// Dummy Notice will added when dummy contractors created
        /// This will help to running or testing this program
        /// </summary>
        /// <param name="_contractor"></param>
        /// <returns></returns>
        public string AddDummyNotice(Contractor _contractor)
        {
            return AddNotice(_contractor);
        }

        /// <summary>
        /// Dummy Notice will added when dummy jobs created
        /// This will help to running or testing this program
        /// </summary>
        /// <param name="_job"></param>
        /// <returns></returns>
        public string AddDummyNotice(Job _job)
        {
            return AddNotice(_job);
        }
    }
}
