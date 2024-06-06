namespace DataModel;

public static class TemplateTransactionalData
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
