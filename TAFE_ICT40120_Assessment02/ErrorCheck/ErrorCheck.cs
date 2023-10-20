using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TAFE_ICT40120_Assessment02.Classes;
using TAFE_ICT40120_Assessment02.Enums;

/// <summary>
/// Error check collection static class 
/// </summary>
public static class ErrorCheck
{
    public static bool CheckValidString(string _target)
    {
        if (CodeCheckValidString(_target))
        {
            return true;
        }
        else { return false; }
    }

    private static bool CodeCheckValidString(string _string)
    {
        if (_string == null) return false;
        if (_string.Length < 2)
        {
            return false;
        }
        if (CheckSpecialLetters(_string) == false)
        {
            return false;
        }


        return true;
    }


    private static bool CheckSpecialLetters(string _string)
    {
        string specialLetters = "~!@#$%^&*()+_=[]{};:'<>?,";
        foreach (char c in specialLetters)
        {
            if (_string.Contains(c))
            {
                return false;
            }
        }
        return true;

    }

    public static bool CheckIsNotNull(Contractor _contractorTarget)
    {
        if (_contractorTarget == null) return false;
        return true;
    }
    public static bool CheckIsNotNull(Job _jobTarget)
    {
        if (_jobTarget == null) return false;
        return true;
    }
    public static bool CheckIsNotNull(Cancel _cancelTarget)
    {
        if (_cancelTarget == null) return false;
        return true;
    }
    public static bool CheckIsNotNull(Completed _completedTarget)
    {
        if (_completedTarget == null) return false;
        return true;
    }




    public static bool CheckComboBoxValidDate(int _comboBoxDay, int _comboBoxMonth, int _comboBoxYear)
    {
        if (_comboBoxMonth == 2)
        {
            if (_comboBoxDay > 28)
            {
                return false;
            }
        }
        int[] months31Dyas = { 1, 3, 5, 7, 8, 10, 12 };
        foreach (int checkMonth in months31Dyas)
        {
            if (_comboBoxMonth == checkMonth) continue;
            if (_comboBoxDay > 30)
            {
                return false;
            }
        }
        return true;
    }


    public static bool CheckComparingLevelBetween(Job _selectedJob, Contractor _selectedJobContractor)
    {
        if (_selectedJob.Level > _selectedJobContractor.Level)
        {
            return false;
        }
        return true;
    }

    public static bool CheckValidDate(DateTime _dateTime)
    {
        if (!CodeValidDate(_dateTime)) return false;
        return true;
    }

    public static bool CheckValidDate(int _year, int _month, int _day)
    {

        if (_year.ToString().Length > 4) return false;
        if (_month.ToString().Length > 2) return false;
        if (_day.ToString().Length > 2) return false;
        try
        {
            if (!CodeValidDate(new DateTime(_year, _month, _day))) return false;
        }
        catch { return false; }
        return true;
    }


    public static bool CodeValidDate(DateTime _dateTime)
    {

        int minimumYear = RecruitmentSystem.minimumYear;
        int currentYear = RecruitmentSystem.currentYear;
        int maxMonth = 12;
        int minMonth = 1;
        int maxDay = 31;
        int minDay = 1;

        if (_dateTime.Year > currentYear) return false;
        if (_dateTime.Year < minimumYear) return false;

        if (_dateTime.Month > maxMonth) return false;
        if (_dateTime.Month < minMonth) return false;

        if (_dateTime.Day > maxDay) return false;
        if (_dateTime.Day < minDay) return false;


        List<int> special29FebraryByYears = new List<int>
            {
                2020,
                2023,
                2025,
                2028
            };

        if (_dateTime.Month == 2)
        {
            if (special29FebraryByYears.Contains(_dateTime.Year))
            {
                maxDay = 29;
            }
            maxDay = 28;
        }




        return true;
    }


    public static double CheckJobCostValue(double _value)
    {
        double max = RecruitmentSystem.maximumJobCost;
        double min = RecruitmentSystem.minimumJobCost;
        double ceilingNumber = _value;

        if (_value > max) return max;
        else if (_value < min) return min;
        ceilingNumber = CodeCeilNumber(ceilingNumber);
        return ceilingNumber;

    }

    public static int CheckValidNumber(string  _value)
    {
        try
        {   
            return int.Parse(_value);
        }
        catch
        {
            return 0;
        }
    }

    public static double CheckContractorWageValue(double _value)
    {
        double max = RecruitmentSystem.maximumContractorWage;
        double min = RecruitmentSystem.minimumContractorWage;

        if (_value > max) return max;
        if (_value < min) return min;
        return _value;

    }

    private static double CodeCeilNumber(double _value)
    {
        double ceilingNumber = _value;
        ceilingNumber /= 10;
        ceilingNumber = Math.Ceiling(ceilingNumber);
        ceilingNumber *= 10;
        return ceilingNumber;
    }


    /// <summary>
    /// Find selected Contractor from comboBox in the list Contractor. 
    /// </summary>
    /// <param name="_contractors"></param>
    /// <param name="_newJob"></param>
    /// <param name="_comboBoxJobContractor"></param>
    public static bool CheckSelectedContractor(List<Contractor> _contractors, Job _newJob, string _comboBoxJobContractor)
    {
        for (int index = _contractors.Count - 1; index >= 0; index--)
        {
            if ($"{_contractors[index].Level} {_contractors[index].FullName}" == _comboBoxJobContractor)
            {
                
                
                return true;
            }
        }
        return false;
    }

}


