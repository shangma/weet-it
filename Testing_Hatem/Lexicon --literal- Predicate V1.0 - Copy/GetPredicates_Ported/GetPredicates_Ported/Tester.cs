﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

namespace GetPredicates_Ported
{
    class Tester
    {
        #region variables
        private static List<List<String>> predicatesURI = new List<List<String>>();
        private static List<List<double>> time = new List<List<double>>();
        private static int position = 2;
        private static Microsoft.Office.Interop.Excel.Application excelapp1, excelapp2;
        private static Microsoft.Office.Interop.Excel.Workbook myworkbook1,myworkbook2;
        private static Microsoft.Office.Interop.Excel.Sheets myworksheets1,myworksheets2;
        private static Microsoft.Office.Interop.Excel.Worksheet myworksheet1,myworksheet2;
        private static Microsoft.Office.Interop.Excel.Range range1,range2;
        private static List<String> excelURIs = new List<String>();
        private static List<List<String>> excelURIsList = new List<List<String>>();
        private static List<List<float>> avg = new List<List<float>>();
        private static int countTrue = 0;
        private static List<double> avgTime = new List<double>();
        private static List<float> totalAvg = new List<float>();
        private static Lexicon mylexicon = new Lexicon();
        private static List<double> tTime=new List<double>();
        private static List<float> aAvg = new List<float>();
        private static List<String> pPredicatesURI = new List<String>();

        private static List<List<List<String>>> allPredicatesURI = new List<List<List<String>>>();
        #endregion

        public static void test(String input, String output)
        {
            openExcel(input, output);
            getURIs();
            getURIsfromExcel();
            comparePredicates();
            avgTimeNResults();
        }
        
        private static void openExcel(String input, String output)
        {
            excelapp1 = new Microsoft.Office.Interop.Excel.Application();
            myworkbook1 = excelapp1.Workbooks.Open(input, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true);
            myworksheets1 = myworkbook1.Worksheets;
            myworksheet1 = myworksheets1.get_Item(1);
            range1 = myworksheet1.UsedRange;

            excelapp2 = new Microsoft.Office.Interop.Excel.Application();
            myworkbook2 = excelapp2.Workbooks.Open(output, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true);
            myworksheets2 = myworkbook2.Worksheets;
            myworksheet2 = myworksheets2.get_Item(1);
            range2 = myworksheet2.UsedRange;
        }

        private static void getURIs()
        { 
        //for (int iLimit = 5; iLimit <= 50; iLimit += 5)
          //  for (int iTopN = 1; (iTopN <= 25 && iTopN < iLimit); iTopN += 3)
            {
                int iLimit = 35;
                int iTopN = 25; 
                myworksheet2.Cells[1][position] = iLimit;
                myworksheet2.Cells[2][position] = iTopN;
                position++;
                tTime = new List<double>();
                predicatesURI = new List<List<String>>();
                for (int i = 2; i <= range1.Rows.Count; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range myrange = myworksheet1.get_Range("A" + i.ToString(), System.Reflection.Missing.Value);
                        String s = myrange.Text;
                        DateTime start = DateTime.Now;
                        List<LexiconPredicate> predicates = mylexicon.getPredicates(s, iTopN,iLimit);
                        DateTime end = DateTime.Now;
                        tTime.Add((end - start).TotalMilliseconds);
                        pPredicatesURI = new List<String>();
                        foreach (LexiconPredicate predicate in predicates)
                        {
                            pPredicatesURI.Add(predicate.URI);
                            util.log(predicate.ToSimpleString());
                        }
                        predicatesURI.Add(pPredicatesURI);
                    }
                    time.Add(tTime);
                    allPredicatesURI.Add(predicatesURI);
            }
        }
        
        private static void getURIsfromExcel()
        {
            for (int i = 2; i <= range1.Rows.Count; i++)
            {
                Microsoft.Office.Interop.Excel.Range myrange = myworksheet1.get_Range("B" + i.ToString(), System.Reflection.Missing.Value);
                excelURIs.Add(myrange.Text);
            }
            foreach (String s in excelURIs)
            {
                String temp = s.Replace(" ", "");
                excelURIsList.Add(temp.Split(',').ToList<String>());
            }
        }

        private static void comparePredicates()
        {
            foreach (List<List<String>> i in allPredicatesURI)
            {
                aAvg = new List<float>();
                foreach(List<String> j in i)
                    foreach (List<String> k in excelURIsList)
                    { 
                        if(i.IndexOf(j)==excelURIsList.IndexOf(k))
                        {
                            int count = 0;
                            foreach (String str1 in k)
                                foreach (String str2 in j)
                                {
                                    if (str1.Equals(str2))
                                        count++;
                                }
                            aAvg.Add(count / k.Count);
                        }
                    }
                avg.Add(aAvg);
            }
        }
        
        private static void avgTimeNResults()
        {
            for (int i = 0; i < avg[0].Count; i++)
            {
                countTrue = 0;
                for (int j = 0; j < avg.Count; j++)
                {
                    if(i==0)
                        totalAvg.Add(0);
                    totalAvg[j] += avg[j][i]/(float)excelURIsList.Count;
                    if (avg[j][i] != 0)
                        countTrue++;
                }
                for (int j = 0; j < avg.Count; j++)
                {
                    if (i == 0)
                        avgTime.Add(0);
                    if (countTrue != 0)
                        avgTime[j] += time[j][i] / (double)countTrue;
                }
            }
            for (int i = 0; i < avg.Count; i++)
            {
                myworksheet2.Cells[3][i+2] = totalAvg[i];
                myworksheet2.Cells[4][i+2] = (int)avgTime[i];
            }
            myworkbook2.Save();
            myworkbook2.Close();
        }
    }
}
