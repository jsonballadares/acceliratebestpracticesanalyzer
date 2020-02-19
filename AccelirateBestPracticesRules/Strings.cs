using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiPath.Studio.RulesLibrary
{
    public class Strings
    {
        /*
         * Naming Rules
         */
        public static string ST_ACC_002_Name = "Lower Camel Case Variable Naming Convention";
        public static string ST_ACC_002_Recommendation = "Consider using camel case when naming your variables if possible";
        public static string ST_ACC_002_DocumentationLink = "Accelirate.com";
        public static string ST_ACC_002_ErrorFormat = "Variable: {0} doesnt match with lowerCamelCase Regex";

        public static string ST_ACC_004_Name = "Activity Action Naming Rule";
        public static string ST_ACC_004_Recommendation = "Activity names should concisely reflect the action taken, (i.e. Click ‘Save’ Button). Keep the part of the title that describe the action (Click, Type Into, Element exists etc.)";
        public static string ST_ACC_004_DocumentationLink = "Accelirate.com";
        public static string ST_ACC_004_ErrorFormat = "Activity Name: {0} should concisely reflect the default action taken: {1}";

        public static string ST_ACC_006_Name = "Process Naming Conventions";
        public static string ST_ACC_006_Recommendation = "Process names should follow Business Process Name and be all lowercase with words separated by underscores. Avoid abbreviations where possible.";
        public static string ST_ACC_006_DocumentationLink = "Accelirate.com";
        public static string ST_ACC_006_ErrorFormat = "{0} should be all lowercase with words seperated by underscores.";

        public static string ST_ACC_009_Name = "General Naming Conventions";
        public static string ST_ACC_009_Recomendation = "The name should contain ONLY alphabetic characters. The name should be derived from a valid name and should be no shorter than 3 characters and not exceed 10 characters.";
        public static string ST_ACC_009_DocumentationLink = "Accelirate.com";
        public static string ST_ACC_009_ErrorFormatActivityVariableName = "Variable: {0} contains non-alphabetic characters.";
        public static string ST_ACC_009_ErrorFormatActivityVariableLength = "Variable: {0} should be no shorter than 3 characters and not exceed 10 characters.";
        
        /*
         * Project Layout Rules
         */
        public static string ST_ACC_010_Name = "Config File Existence";
        public static string ST_ACC_010_Recommendation = "The project directory should contain a Config file (JSON or Excel), accessible outside of the source code, where any dynamic values should be assigned for use within the source code.";
        public static string ST_ACC_010_DocumentationLink = "Accelirate.com";
    }
}
