using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Reporting.Tests;

public class TransactionalData
{
    public string LineOfBusiness { get; set; } = string.Empty;

    public string AmountType { get; set; } = string.Empty;

    public string Scenario { get; set; } = string.Empty;

    public string AocType { get; set; } = string.Empty;

    public string Currency { get; set; } = string.Empty;

    public double Value { get; set; }

}

public static class TemplateData
{
    public static TransactionalData[] GetTransactionalData() => [
        new() { AmountType = "MAI", AocType = "BOP", Currency = "CHF", LineOfBusiness = "DIS", Value = 10 },
        new() { AmountType = "MAI", AocType = "BOP", Currency = "CHF", LineOfBusiness = "AEH", Value = 10 },
        new() { AmountType = "ACQ", AocType = "BOP", Currency = "CHF", LineOfBusiness = "ENE", Value = 10 },
        new() { AmountType = "ACQ", AocType = "BOP", Currency = "CHF", LineOfBusiness = "MAA", Value = 10 },
        new() { AmountType = "NIC", AocType = "BOP", Currency = "CHF", LineOfBusiness = "PRO", Value = 10 },
        new() { AmountType = "NIC", AocType = "BOP", Currency = "CHF", LineOfBusiness = "LIA", Value = 10 },
        new() { AmountType = "CLE", AocType = "BOP", Currency = "CHF", LineOfBusiness = "AEH", Value = 10 },
        new() { AmountType = "CLE", AocType = "BOP", Currency = "CHF", LineOfBusiness = "AEH", Value = 10 },
        new() { AmountType = "CLE", AocType = "BOP", Currency = "CHF", LineOfBusiness = "OLI", Value = 10 },
        new() { AmountType = "CLE", AocType = "BOP", Currency = "CHF", LineOfBusiness = "ONL", Value = 10 },
        new() { AmountType = "PR",  AocType = "BOP", Currency = "CHF", LineOfBusiness = "MOT", Value = 10 },
        new() { AmountType = "PRI", AocType = "BOP", Currency = "CHF", LineOfBusiness = "PRO", Value = 10 },
        new() { AmountType = "PR",  AocType = "BOP", Currency = "CHF", LineOfBusiness = "MAA", Value = 10 },
        new() { AmountType = "PR",  AocType = "BOP", Currency = "CHF", LineOfBusiness = "LIA", Value = 10 },
        new() { AmountType = "PR",  AocType = "BOP", Currency = "CHF", LineOfBusiness = "AEH", Value = 10 },
        ];
}
