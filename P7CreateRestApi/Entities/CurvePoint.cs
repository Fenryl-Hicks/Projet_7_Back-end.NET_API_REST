using System;

namespace P7CreateRestApi.Entities
{
    public class CurvePoint
    {
        int Id;
        byte? CurveId;
        DateTime? AsOfDate;
        double? Term;
        double? CurvePointValue;
        DateTime? CreationDate;
    }
}