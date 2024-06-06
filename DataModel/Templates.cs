using System.IO.Enumeration;
using System.Security.Claims;

namespace DataModel;

public static class Templates
{
    public static LineOfBusiness[] GetLineOfBusinesses() => [
        new() { SystemName = "M",   DisplayName = "Life and Non-Life",  Parent = "" },
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
        new() { SystemName = "CL",  DisplayName = "Claims",                   Parent = "" },
        new() { SystemName = "PR",  DisplayName = "Premiums",                 Parent = "" },
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
}
