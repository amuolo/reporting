namespace DataModel;

public static class TemplateDimensions
{
    public static LineOfBusiness[] GetLineOfBusinesses() => [
        new() { SystemName = "M",   DisplayName = "Life and Non-Life",  Parent = null },
        new() { SystemName = "LI",  DisplayName = "Life",               Parent = "M" },                       
        new() { SystemName = "NL",  DisplayName = "Non-Life",           Parent = "M" },                    
        new() { SystemName = "LIA", DisplayName = "Liability",          Parent = "NL" },                  
        new() { SystemName = "MAA", DisplayName = "Marine, Aviation",   Parent = "NL" },            
        new() { SystemName = "ENE", DisplayName = "Energy",             Parent = "NL" },            
        new() { SystemName = "MOT", DisplayName = "Motor",              Parent = "NL" },            
        new() { SystemName = "PRO", DisplayName = "Property",           Parent = "NL" },            
        new() { SystemName = "ENG", DisplayName = "Engineering",        Parent = "NL" },            
        new() { SystemName = "ONL", DisplayName = "Other Non-Life",     Parent = "NL" },            
        new() { SystemName = "DIS", DisplayName = "Disability",         Parent = "LI" },            
        new() { SystemName = "AEH", DisplayName = "Accident & Health",  Parent = "LI" },          
        new() { SystemName = "OLI", DisplayName = "Other Life",         Parent = "LI" },          
        ];

    public static AmountType[] GetAmountTypes() => [
        new() { SystemName = "CL",  DisplayName = "Claims",                   Parent = null },
        new() { SystemName = "PR",  DisplayName = "Premiums",                 Parent = null },
        new() { SystemName = "CLE", DisplayName = "Claim Expenses",           Parent = "CL" },               
        new() { SystemName = "NIC", DisplayName = "Non-investment component", Parent = "CL" },      
        new() { SystemName = "ICO", DisplayName = "Investment component",     Parent = "CL" },      
        new() { SystemName = "ACQ", DisplayName = "Acquisition",              Parent = "CL" },      
        new() { SystemName = "MAI", DisplayName = "Maintenance",              Parent = "CL" },      
        ];

    public static Scenario[] GetScenarios() => [
        new() { SystemName = "",           DisplayName = "Best Estimate" },
        new() { SystemName = "YCUP1.0pct", DisplayName = "Yield Curve Up 1.0pct" },
        new() { SystemName = "SRUP1.0pct", DisplayName = "Spread Rate Up 1.0pct" },
        new() { SystemName = "EUP1.0pct",  DisplayName = "Equity Up 1.0pct" },
        new() { SystemName = "FXUP1.0pct", DisplayName = "Exchange Rate Up 1.0pct" },
        new() { SystemName = "MTUP10pct",  DisplayName = "Mortality Up 10pct" }, 
        new() { SystemName = "LUP10pct",   DisplayName = "Longevity Up 10pct" },
        ];

    public static AocType[] GetAocTypes() => [
        new() { SystemName = "BOP",  DisplayName = "Beginning of Period" },
        new() { SystemName = "MC",   DisplayName = "Model Correction" },
        new() { SystemName = "PC",   DisplayName = "Portfolio Changes" },
        new() { SystemName = "CF",   DisplayName = "Cash Flow" },
        new() { SystemName = "IA",   DisplayName = "Interest Accretion" },
        new() { SystemName = "AU",   DisplayName = "Assumption Update" },
        new() { SystemName = "FAU",  DisplayName = "Financial Assumption Update" },
        new() { SystemName = "YCU",  DisplayName = "Yield Curve Update" },                // Parent = "FAU"
        new() { SystemName = "CRU",  DisplayName = "Credit Risk Update" },                // Parent = "CRU"
        new() { SystemName = "EV",   DisplayName = "Experience Variance" },
        new() { SystemName = "WO",   DisplayName = "Write Off" },
        new() { SystemName = "CL",   DisplayName = "Combined Liabilities" },
        new() { SystemName = "EA",   DisplayName = "Experience Adjustment" },
        new() { SystemName = "AM",   DisplayName = "Amortization" },
        new() { SystemName = "FX",   DisplayName = "FX Impact" },
        new() { SystemName = "EOP",  DisplayName = "End Of Period" },
        ];
}
