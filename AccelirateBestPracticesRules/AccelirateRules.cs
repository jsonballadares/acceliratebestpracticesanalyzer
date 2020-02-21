using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using UiPath.Studio.Activities.Api;
using UiPath.Studio.Activities.Api.Analyzer;
using UiPath.Studio.Activities.Api.Analyzer.Rules;
using UiPath.Studio.Analyzer.Models;

namespace UiPath.Studio.RulesLibrary
{
    public class RegisterAnalyzerConfiguration : IRegisterAnalyzerConfiguration
    {
        /*
         * Naming Rules
         */
        internal static class AccelirateVariableNamingRuleLowerCamelCase 
        {
            private const string RuleId = "ST_ACC_002";
            //regex for matching ONLY lowerCamelCase
            private const string lowerCamelCaseRegex = @"^[a-z]+[A-Z]+[a-zA-z]*";

            internal static Rule<IActivityModel> Get()
            {
                var Recommendation = Strings.ST_ACC_002_Recommendation;
                var rule = new Rule<IActivityModel>(Strings.ST_ACC_002_Name, RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                    /// Off and Verbose are not supported.

                };
                return rule;
            }

            private static InspectionResult Inspect(IActivityModel activityModel, Rule ruleInstance)
            {

                /*
                 * possible improvement would be making a better lowerCamelCase regex pattern
                 * maybe introduce logic to break my word and then check beginning of each word after first
                 * to see if it is uppercase etc
                 */
                var regex = new Regex(lowerCamelCaseRegex);
                var messageList = new List<string>();
                foreach (var activityModelVariable in activityModel.Variables)
                {
                    if (!regex.IsMatch(activityModelVariable.DisplayName))
                    {
                        messageList.Add(string.Format(Strings.ST_ACC_002_ErrorFormat, activityModelVariable.DisplayName));
                        //messageList.Add(string.Format("activityModelVariable default value: {0} activityModelVariable defined expression: {1} activityModelVariable display name: {2} activityModelVariable type: {3}", activityModelVariable.DefaultValue, activityModelVariable.DefinedExpression, activityModelVariable.DisplayName, activityModelVariable.Type));
                    }
                }

                if (messageList.Count > 0)
                {
                    return new InspectionResult()
                    {
                        ErrorLevel = ruleInstance.ErrorLevel,
                        HasErrors = true,
                        RecommendationMessage = ruleInstance.RecommendationMessage,
                        Messages = messageList,
                        DocumentationLink = ruleInstance.DocumentationLink
                    };
                }
                else
                {
                    return new InspectionResult() { HasErrors = false };
                }
            }
        }
        internal static class AccelirateActivityNamingRuleReflectAction
        {
            //play around with naming standard
            private const string RuleId = "ST_ACC_004";

            internal static Rule<IActivityModel> Get()
            {
                var Recommendation = Strings.ST_ACC_004_Recommendation;
                var DocumentationLink = Strings.ST_ACC_004_DocumentationLink;
                var rule = new Rule<IActivityModel>(Strings.ST_ACC_004_Name, RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                    DocumentationLink = DocumentationLink,
                };
                return rule;
            }

            private static InspectionResult Inspect(IActivityModel activityModel, Rule ruleInstance)
            {
                var messageList = new List<string>();
                var str = activityModel.Type;
                string defaultDisplayName = (str.Split(',')[0]).Split('.')[(str.Split(',')[0]).Split('.').Length - 1];


                //the current DisplayName shouldn't be equal to the default display name but it should atleast contain it
                if (activityModel.DisplayName.ToLower().Equals(defaultDisplayName.ToLower()) || !activityModel.DisplayName.ToLower().Contains(defaultDisplayName.ToLower()))
                {
                    messageList.Add(string.Format(Strings.ST_ACC_004_ErrorFormat, activityModel.DisplayName, defaultDisplayName));
                }

                if (messageList.Count > 0)
                {
                    return new InspectionResult()
                    {
                        ErrorLevel = ruleInstance.ErrorLevel,
                        HasErrors = true,
                        RecommendationMessage = ruleInstance.RecommendationMessage,
                        Messages = messageList,
                        DocumentationLink = ruleInstance.DocumentationLink
                    };
                }
                else
                {
                    return new InspectionResult() { HasErrors = false };
                }
            }
        }
        internal static class AccelirateProcessNamingConvention
        {
            //play around with naming standard
            private const string RuleId = "ST_ACC_006";
            //regex that process names should follow
            private const string processNameRegex = @"^[a-z]+([_]+[a-z]+)+";
            internal static Rule<IProjectModel> Get()
            {
                var Recommendation = Strings.ST_ACC_006_Recommendation;
                var rule = new Rule<IProjectModel>(Strings.ST_ACC_006_Name, RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                };
                return rule;
            }

            private static InspectionResult Inspect(IProjectModel projectModel, Rule ruleInstance)
            {

                var messageList = new List<string>();
                var regex = new Regex(processNameRegex);
                /*
                 * Make a better algorithm to split by words and possibly auto create the given porcess name in the desired lower_case_and_underscore formatting
                 * use regex or logic
                 * right now we just check to see if it has an underscore and if the name is lowercase which is flawed
                 * regex would work well for the checking of _ as what if the name cant have an underscore?
                 */

                //var projectName = ((IProjectSummary)projectModel).DisplayName;
                var projectName = ((IInspectionObject)projectModel).DisplayName;

                if (!regex.IsMatch(projectName))
                {
                    messageList.Add(string.Format(Strings.ST_ACC_006_ErrorFormat, projectName));
                }

                if (messageList.Count > 0)
                {
                    return new InspectionResult()
                    {
                        ErrorLevel = ruleInstance.ErrorLevel,
                        HasErrors = true,
                        RecommendationMessage = ruleInstance.RecommendationMessage,
                        Messages = messageList,
                        DocumentationLink = ruleInstance.DocumentationLink
                    };
                }
                else
                {
                    return new InspectionResult() { HasErrors = false };
                }
            }
        }
        internal static class AccelirateGeneralNamingConventionActivity
        {
            //play around with naming standard
            private const string RuleId = "ST_ACC_009";
            //regex for matching ONLY alphabet characters
            private const string alphabeticRegex = @"^[A-Za-z]+$";
            //min and max lengths
            private const string minimumLength = "3";
            private const string maximumLength = "10";
            //keys
            private const string minimumKey = "min";
            private const string maximumKey = "max";
            internal static Rule<IActivityModel> Get()
            {
                var Recommendation = Strings.ST_ACC_009_Recomendation;
                var rule = new Rule<IActivityModel>(Strings.ST_ACC_009_Name, RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                };

                var paramOne = new Parameter()
                {
                    LocalizedDisplayName = "Minimum Length"
                };
                var paramTwo = new Parameter()
                {
                    LocalizedDisplayName = "Maximum Length"
                };
                rule.Parameters.Add(minimumKey, paramOne);
                rule.Parameters.Add(maximumKey, paramTwo);
                return rule;
            }

            private static InspectionResult Inspect(IActivityModel activityModel, Rule ruleInstance)
            {
                var regex = new Regex(alphabeticRegex);
                var messageList = new List<string>();

                // This retrieves the parameter value from the rule instance as configured by the user, if not, the default value.
                var min = ruleInstance.Parameters[minimumKey].Value;
                var max = ruleInstance.Parameters[maximumKey].Value;

                int lowerBound,upperBound;

                if (min == null)
                {
                    lowerBound = Int32.Parse(minimumLength);
                }
                else
                {
                    lowerBound = Int32.Parse(min);
                }

                if (max == null)
                {
                    upperBound = Int32.Parse(maximumLength);
                }
                else
                {
                    upperBound = Int32.Parse(max);
                }


                foreach (var activityModelVariable in activityModel.Variables)
                {
                    //The name should contain ONLY alphabetic characters. 
                    if (!regex.IsMatch(activityModelVariable.DisplayName))
                    {
                        messageList.Add(string.Format(Strings.ST_ACC_009_ErrorFormatActivityVariableName, activityModelVariable.DisplayName));
                    }

                    //checking if variables they fall within length restraints
                    if (activityModelVariable.DisplayName.Length < lowerBound || activityModelVariable.DisplayName.Length > upperBound)
                    {
                        messageList.Add(string.Format(Strings.ST_ACC_009_ErrorFormatActivityVariableLength, activityModelVariable.DisplayName,lowerBound,upperBound));
                    }
                }

                if (messageList.Count > 0)
                {
                    return new InspectionResult()
                    {
                        ErrorLevel = ruleInstance.ErrorLevel,
                        HasErrors = true,
                        RecommendationMessage = ruleInstance.RecommendationMessage,
                        Messages = messageList,
                        DocumentationLink = ruleInstance.DocumentationLink
                    };
                }
                else
                {
                    return new InspectionResult() { HasErrors = false };
                }
            }
        }
        internal static class AccelirateGeneralNamingConventionWorkflow
        {
            //play around with naming standard
            private const string RuleId = "ST_ACC_009";
            //regex for matching ONLY alphabet characters
            private const string alphabeticRegex = @"^[A-Za-z]+$";
            internal static Rule<IWorkflowModel> Get()
            {
                var Recommendation = Strings.ST_ACC_009_Recomendation;
                var rule = new Rule<IWorkflowModel>(Strings.ST_ACC_009_Name, RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                };
                return rule;
            }

            private static InspectionResult Inspect(IWorkflowModel workflowModel, Rule ruleInstance)
            {
                var regex = new Regex(alphabeticRegex);
                var messageList = new List<string>();



                if (workflowModel.DisplayName.Length < 3 || workflowModel.DisplayName.Length > 10)
                {
                    messageList.Add(string.Format("Workflow Name: {0} should be no shorter than 3 characters and not exceed 10 characters.", workflowModel.DisplayName));
                }

                //The name should contain ONLY alphabetic characters. 
                if (!regex.IsMatch(workflowModel.DisplayName))
                {
                    messageList.Add(string.Format("Workflow Name: {0} doesnt match with alphabetic regex.", workflowModel.DisplayName));
                }

                foreach (var workflowArgs in workflowModel.Arguments)
                {
                    //The name should contain ONLY alphabetic characters. 
                    if (!regex.IsMatch(workflowArgs.DisplayName))
                    {
                        messageList.Add(string.Format("Workflow Argument: {0} doesnt match with alphabetic regex.", workflowArgs.DisplayName));
                    }

                    //checking if variables they fall within length restraints
                    if (workflowArgs.DisplayName.Length < 3 || workflowArgs.DisplayName.Length > 10)
                    {
                        messageList.Add(string.Format("Workflow Argument: {0} should be no shorter than 3 characters and not exceed 10 characters.", workflowArgs.DisplayName));
                    }
                }

                if (messageList.Count > 0)
                {
                    return new InspectionResult()
                    {
                        ErrorLevel = ruleInstance.ErrorLevel,
                        HasErrors = true,
                        RecommendationMessage = ruleInstance.RecommendationMessage,
                        Messages = messageList,
                        DocumentationLink = ruleInstance.DocumentationLink
                    };
                }
                else
                {
                    return new InspectionResult() { HasErrors = false };
                }
            }
        }
        internal static class AccelirateGeneralNamingConventionProject
        {
            //play around with naming standard
            private const string RuleId = "ST_ACC_009";
            //regex for matching ONLY alphabet characters
            private const string alphabeticRegex = @"^[A-Za-z]+$";
            internal static Rule<IProjectModel> Get()
            {
                var Recommendation = Strings.ST_ACC_009_Recomendation;
                var rule = new Rule<IProjectModel>(Strings.ST_ACC_009_Name, RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                };
                return rule;
            }

            private static InspectionResult Inspect(IProjectModel projectModel, Rule ruleInstance)
            {
                var regex = new Regex(alphabeticRegex);
                var messageList = new List<string>();

                var projectName = ((IInspectionObject)projectModel).DisplayName;

                //the name should be in this range
                if (projectName.Length < 3 || projectName.Length > 10)
                {
                    messageList.Add(string.Format("Project Name: {0} should be no shorter than 3 characters and not exceed 10 characters.", projectName));
                }

                //The name should contain ONLY alphabetic characters. 
                if (!regex.IsMatch(projectName))
                {
                    messageList.Add(string.Format("Project Name: {0} doesnt match with alphabetic regex.", projectName));
                }


                if (messageList.Count > 0)
                {
                    return new InspectionResult()
                    {
                        ErrorLevel = ruleInstance.ErrorLevel,
                        HasErrors = true,
                        RecommendationMessage = ruleInstance.RecommendationMessage,
                        Messages = messageList,
                        DocumentationLink = ruleInstance.DocumentationLink
                    };
                }
                else
                {
                    return new InspectionResult() { HasErrors = false };
                }
            }
        }
 
        
        /*
         * Project Layout Rules
         */
        internal static class AccelirateProjectDirectoryHasConfig
        {
            //play around with naming standard
            private const string RuleId = "ST_ACC_011";

            internal static Rule<IProjectModel> Get()
            {
                var DocumentationLink = String.Format("Blah blah blah blah. Click {0} for more information.",
                    "<a href=\"http://www.Accelirate.com\">here</a>");
                var Recommendation = "The project directory should contain a Config file (JSON or Excel), accessible outside of the source code, where any dynamic values should be assigned for use within the source code.  ";
                var rule = new Rule<IProjectModel>("Config File Existence", RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                    DocumentationLink = DocumentationLink,
                };
                return rule;
            }

            private static InspectionResult Inspect(IProjectModel projectModel, Rule ruleInstance)
            {
                var messageList = new List<string>();


                var projectFilePath = projectModel.ProjectFilePath;
                var projectDirectoryPath = projectFilePath.Remove(projectFilePath.IndexOf(@"\project.json"));



                /*
                 * This can be done but implementation is sloppy ask cameron what exactly should be the "project directory" 
                 */

                string[] jsonFiles = Directory.GetFiles(projectDirectoryPath, "*.json", SearchOption.AllDirectories); //Gathers array of all JSON files in directory
                string[] xlsxFiles = Directory.GetFiles(projectDirectoryPath, "*.xlsx", SearchOption.AllDirectories); //Gathers array of all XLSX files in directory

                var fileNames = new List<string>();

                foreach (var file in jsonFiles)
                {
                    fileNames.Add(Path.GetFileName(file));
                }

                foreach (var file in xlsxFiles)
                {
                    fileNames.Add(Path.GetFileName(file));
                }

                var configExist = false;
                foreach (var fileName in fileNames)
                {
                    if (fileName.Contains("config") || fileName.Contains("Config"))
                    {
                        configExist = true;
                        break;
                    }
                }

                if (!configExist)
                {
                    messageList.Add(string.Format("Config file doesnt exist please make one!"));
                }

                if (messageList.Count > 0)
                {
                    return new InspectionResult()
                    {
                        ErrorLevel = ruleInstance.ErrorLevel,
                        HasErrors = true,
                        RecommendationMessage = ruleInstance.RecommendationMessage,
                        Messages = messageList,
                        DocumentationLink = ruleInstance.DocumentationLink
                    };
                }
                else
                {
                    return new InspectionResult() { HasErrors = false };
                }
            }
        }
        internal static class AccelirateProjectRootDirectoryClean
        {
            //play around with naming standard
            private const string RuleId = "ST_ACC_012";

            internal static Rule<IProjectModel> Get()
            {
                var DocumentationLink = String.Format("Blah blah blah blah. Click {0} for more information.",
                    "<a href=\"http://www.Accelirate.com\">here</a>");
                var Recommendation = "The project directory should contain a Config file (JSON or Excel), accessible outside of the source code, where any dynamic values should be assigned for use within the source code.  ";
                var rule = new Rule<IProjectModel>("Config File Existence", RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                    DocumentationLink = DocumentationLink,
                };
                return rule;
            }

            private static InspectionResult Inspect(IProjectModel projectModel, Rule ruleInstance)
            {
                var messageList = new List<string>();

                var projectFilePath = projectModel.ProjectFilePath;
                var projectDirectoryPath = projectFilePath.Remove(projectFilePath.IndexOf(@"\project.json"));



                foreach (var file in projectModel.FileNames)
                {
                    var xamlFileDirectory = file.Remove(file.IndexOf(@"\" + file.Split('\\')[file.Split('\\').Length - 1]));
                    if (xamlFileDirectory.Equals(projectDirectoryPath) && !file.ToLower().Equals(projectModel.EntryPointName.ToLower()))
                    {
                        messageList.Add(file.ToString() + " shouldnt be in the project directory: " + projectDirectoryPath);
                    }
                }

                /*
                 * This can be done but implementation is sloppy ask cameron what exactly should be the "project directory" 
                 * The on
                 */

                //Gathers array of all XAML files in directory which according to our standards Main.xaml should be here so there should always be atleast one file
                //var files = Directory.GetFiles(projectDirectoryPath, "*.xaml"); 


                //if (files.Length > 1)   
                //{
                //    messageList.Add(string.Format("The Main.xaml should be the only files in the Project root directory."));
                //}

                if (messageList.Count > 0)
                {
                    return new InspectionResult()
                    {
                        ErrorLevel = ruleInstance.ErrorLevel,
                        HasErrors = true,
                        RecommendationMessage = ruleInstance.RecommendationMessage,
                        Messages = messageList,
                        DocumentationLink = ruleInstance.DocumentationLink
                    };
                }
                else
                {
                    return new InspectionResult() { HasErrors = false };
                }
            }
        }
        internal static class AccelirateProjectFolderNames
        {
            //play around with naming standard
            private const string RuleId = "ST_ACC_013";
            private const string directoryNameRegex = @"^([a-z]+([_]+[a-z]+)+)";
            internal static Rule<IProjectModel> Get()
            {
                var DocumentationLink = String.Format("Blah blah blah blah. Click {0} for more information.",
                    "<a href=\"http://www.Accelirate.com\">here</a>");
                var Recommendation = "The name should contain only alphabetic characters and represent the purpose or application. (E. g. adobe)The name length should be no shorter than 3 characters and not exceed 30 characters. Names should be all lower case with words separated by underscores  ";
                var rule = new Rule<IProjectModel>("Config File Existence", RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                    DocumentationLink = DocumentationLink,
                };
                return rule;
            }

            private static InspectionResult Inspect(IProjectModel projectModel, Rule ruleInstance)
            {
                var messageList = new List<string>();

                var projectFilePath = projectModel.ProjectFilePath;
                var projectDirectoryPath = projectFilePath.Remove(projectFilePath.IndexOf(@"\project.json"));

                var directoryNames = Directory.GetDirectories(projectDirectoryPath).Select(Path.GetFileName).ToArray();

                var regex = new Regex(directoryNameRegex);

                foreach (var directoryName in directoryNames)
                {
                    //ignore hidden files and then enforce naming pattern
                    if (!directoryName.StartsWith(".") && !regex.IsMatch(directoryNameRegex))
                    {
                        messageList.Add(directoryName + " should be all lowercase and seperated by underscores contain only alphabetic characters.");
                    }

                    //enforce file lengths
                    if (directoryName.Length < 3)
                    {
                        messageList.Add(directoryName + " should be no shorter than 3 characters");
                    }

                    if (directoryName.Length > 30)
                    {
                        messageList.Add(directoryName + " should not exceed 30 characters.");
                    }

                }



                if (messageList.Count > 0)
                {
                    return new InspectionResult()
                    {
                        ErrorLevel = ruleInstance.ErrorLevel,
                        HasErrors = true,
                        RecommendationMessage = ruleInstance.RecommendationMessage,
                        Messages = messageList,
                        DocumentationLink = ruleInstance.DocumentationLink
                    };
                }
                else
                {
                    return new InspectionResult() { HasErrors = false };
                }
            }
        }
        //internal static class AccelirateProjectModuleNames
        //{
        //    //play around with naming standard
        //    private const string RuleId = "ST_ACC_014";
        //    private const string moduleNameRegex = @"^([a-z]+([_]+[a-z]+)+)";
        //    internal static Rule<IProjectModel> Get()
        //    {
        //        var DocumentationLink = String.Format("Blah blah blah blah. Click {0} for more information.",
        //            "<a href=\"http://www.Accelirate.com\">here</a>");
        //        var Recommendation = "The name should contain only alphabetic characters and represent the purpose or application. (E. g. adobe)The name length should be no shorter than 3 characters and not exceed 30 characters. Names should be all lower case with words separated by underscores  ";
        //        var rule = new Rule<IProjectModel>("Config File Existence", RuleId, Inspect)
        //        {
        //            ErrorLevel = System.Diagnostics.TraceLevel.Warning,
        //            RecommendationMessage = Recommendation,
        //            DocumentationLink = DocumentationLink,
        //        };
        //        return rule;
        //    }

        //    private static InspectionResult Inspect(IProjectModel projectModel, Rule ruleInstance)
        //    {
        //        var messageList = new List<string>();

        //        var regex = new Regex(moduleNameRegex);

        //        foreach (var workflow in projectModel.Workflows)
        //        {

        //            if (!workflow.DisplayName.ToLower().Equals("main.xaml") && !regex.IsMatch(moduleNameRegex))
        //            {
        //                messageList.Add(workflow.DisplayName + " should be all lowercase and seperated by underscores contain only alphabetic characters.");
        //            }

        //            //enforce file lengths
        //            if (workflow.DisplayName.Length < 3)
        //            {
        //                messageList.Add(workflow.DisplayName + " should be no shorter than 3 characters.");
        //            }

        //            if (workflow.DisplayName.Length > 30)
        //            {
        //                messageList.Add(workflow.DisplayName + " should not exceed 30 characters.");
        //            }

        //        }



        //        if (messageList.Count > 0)
        //        {
        //            return new InspectionResult()
        //            {
        //                ErrorLevel = ruleInstance.ErrorLevel,
        //                HasErrors = true,
        //                RecommendationMessage = ruleInstance.RecommendationMessage,
        //                Messages = messageList,
        //                DocumentationLink = ruleInstance.DocumentationLink
        //            };
        //        }
        //        else
        //        {
        //            return new InspectionResult() { HasErrors = false };
        //        }
        //    }
        //}
        internal static class AccelirateProjectModuleNames
        {
            //play around with naming standard
            private const string RuleId = "ST_ACC_014";
            private const string moduleNameRegex = @"^([a-z]+([_]+[a-z]+)+)";
            internal static Rule<IWorkflowModel> Get()
            {
                var DocumentationLink = String.Format("Blah blah blah blah. Click {0} for more information.",
                    "<a href=\"http://www.Accelirate.com\">here</a>");
                var Recommendation = "The name should contain only alphabetic characters and represent the purpose or application. (E. g. adobe)The name length should be no shorter than 3 characters and not exceed 30 characters. Names should be all lower case with words separated by underscores  ";
                var rule = new Rule<IWorkflowModel>("Module Naming Rule", RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                    DocumentationLink = DocumentationLink,
                };
                return rule;
            }

            private static InspectionResult Inspect(IWorkflowModel workflowModel, Rule ruleInstance)
            {
                var messageList = new List<string>();

                var regex = new Regex(moduleNameRegex);


                if (workflowModel.DisplayName.ToLower().Equals("main.xaml") && !regex.IsMatch(workflowModel.DisplayName))
                {
                    messageList.Add(workflowModel.DisplayName + " should be all lowercase and seperated by underscores contain only alphabetic characters.");
                }

                //enforce file lengths
                if (workflowModel.DisplayName.Length < 3)
                {
                    messageList.Add(workflowModel.DisplayName + " should be no shorter than 3 characters.");
                }

                if (workflowModel.DisplayName.Length > 30)
                {
                    messageList.Add(workflowModel.DisplayName + " should not exceed 30 characters.");
                }





                if (messageList.Count > 0)
                {
                    return new InspectionResult()
                    {
                        ErrorLevel = ruleInstance.ErrorLevel,
                        HasErrors = true,
                        RecommendationMessage = ruleInstance.RecommendationMessage,
                        Messages = messageList,
                        DocumentationLink = ruleInstance.DocumentationLink
                    };
                }
                else
                {
                    return new InspectionResult() { HasErrors = false };
                }
            }
        }

        /*
        * Tests to show data from UiPath using Api for Analyzer
        */
        internal static class AccelirateParameterExample
        {
            //play around with naming standard
            private const string RuleId = "PARAMETER_ACC_EXAMPLE";
            private const string lowerRange = "lowerRange";
            private const string upperRange = "upperRange";
            private const string defaultValue = "777";

            internal static Rule<IActivityModel> Get()
            {
                var Recommendation = "PARAMETER EXAMPLE";
                var rule = new Rule<IActivityModel>("PARAMETER", RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                };
                var paramOne = new Parameter()
                {
                    Key = lowerRange,
                    Value = defaultValue, //doesnt seem to work
                    LocalizedDisplayName = "Lower Range"
                };
                var paramTwo = new Parameter()
                {
                    Key = upperRange,
                    Value = defaultValue,
                    LocalizedDisplayName = "Upper Range"
                };
                rule.Parameters.Add(lowerRange, paramOne);
                rule.Parameters.Add(upperRange, paramTwo);
                return rule;
            }

            private static InspectionResult Inspect(IActivityModel activityModel, Rule ruleInstance)
            {

                var messageList = new List<string>();

                // This retrieves the parameter value from the rule instance as configured by the user, if not, the default value.
                var lowerBound = ruleInstance.Parameters[lowerRange].Value;
                var upperBound = ruleInstance.Parameters[upperRange].Value;

                /*
                 * to get the default value just check if the parameters are null and then set it to whatever
                 */
                if(lowerBound == null)
                {
                    messageList.Add(defaultValue);
                }

                if (upperBound == null)
                {
                    messageList.Add(defaultValue);
                }

                messageList.Add("ruleInstance.Parameters[lowerRange].Value: " + ruleInstance.Parameters[lowerRange].Value);
                messageList.Add("ruleInstance.Parameters[upperRange].Value: " + ruleInstance.Parameters[upperRange].Value);
                messageList.Add("ruleInstance.Parameters[lowerRange].Key: " + ruleInstance.Parameters[lowerRange].Key);
                messageList.Add("ruleInstance.Parameters[upperRange].Key: " + ruleInstance.Parameters[upperRange].Key);
                messageList.Add("LOWER BOUND: " + lowerBound);
                messageList.Add("UPPER BOUND: " + upperBound);


                if (messageList.Count > 0)
                {
                    return new InspectionResult()
                    {
                        
                        ErrorLevel = ruleInstance.ErrorLevel,
                        HasErrors = true,
                        RecommendationMessage = ruleInstance.RecommendationMessage,
                        Messages = messageList,
                        DocumentationLink = "google.com"
                    };
                }
                else
                {
                    return new InspectionResult() { HasErrors = false };
                }
            }
        }
        internal static class AccelirateProjectApiTest
        {
            //play around with naming standard
            private const string RuleId = "PROJECT_API_TEST";

            internal static Rule<IProjectModel> Get()
            {
                var DocumentationLink = String.Format("Blah blah blah blah. Click {0} for more information.",
                    "<a href=\"http://www.Accelirate.com\">here</a>");
                var Recommendation = "Testing the project interface";
                var rule = new Rule<IProjectModel>("Project Interface Test", RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                    DocumentationLink = DocumentationLink,
                };
                return rule;
            }

            private static InspectionResult Inspect(IProjectModel projectModel, Rule ruleInstance)
            {
                var messageList = new List<string>();

                //stuff that deals with just the project
                messageList.Add(string.Format("Project DisplayName: {0}", ((IInspectionObject)projectModel).DisplayName));
                messageList.Add(string.Format("Project EntryPointName: {0}", projectModel.EntryPointName));
                messageList.Add(string.Format("Project ExceptionHandlerWorkflowName: {0}", projectModel.ExceptionHandlerWorkflowName));
                messageList.Add(string.Format("Project ExpressionLanguage: {0}", projectModel.ExpressionLanguage));
                messageList.Add(string.Format("Project FilePath: {0}", projectModel.ProjectFilePath));
                messageList.Add(string.Format("Project ProjectOutputType: {0}", projectModel.ProjectOutputType));
                messageList.Add(string.Format("Project ProjectProfileType: {0}", projectModel.ProjectProfileType));
                messageList.Add(string.Format("projectModel.RequiresUserInteraction: {0}", projectModel.RequiresUserInteraction));
                messageList.Add(string.Format("projectModel.SupportsPersistence: {0}", projectModel.SupportsPersistence));

                messageList.Add(string.Format("projectModel.EntryPoint.DisplayName: {0}", projectModel.EntryPoint.DisplayName));
                messageList.Add(string.Format("projectModel.EntryPoint.RelativePath.ToString(): {0}", projectModel.EntryPoint.RelativePath.ToString()));
                messageList.Add(string.Format("projectModel.EntryPoint.Root.DisplayName: {0}", projectModel.EntryPoint.Root.DisplayName));

                foreach (var dependency in projectModel.Dependencies)
                {
                    messageList.Add(string.Format("Dependency: {0}", dependency.Name));
                }

                foreach (var file in projectModel.FileNames)
                {
                    messageList.Add(string.Format("File: {0}", file.ToString()));

                }

                foreach (var workflow in projectModel.Workflows)
                {
                    messageList.Add(string.Format("workflow.DisplayName: {0}", workflow.DisplayName));
                    messageList.Add(string.Format("workflow.RelativePath: {0}", workflow.RelativePath));
                    messageList.Add(string.Format("workflow.Root.DisplayName: {0}", workflow.Root.DisplayName));
                    messageList.Add(string.Format("workflow.Arguments.Count: {0}", workflow.Arguments.Count));
                }

                if (messageList.Count > 0)
                {
                    return new InspectionResult()
                    {
                        ErrorLevel = ruleInstance.ErrorLevel,
                        HasErrors = true,
                        RecommendationMessage = ruleInstance.RecommendationMessage,
                        Messages = messageList,
                        DocumentationLink = ruleInstance.DocumentationLink
                    };
                }
                else
                {
                    return new InspectionResult() { HasErrors = false };
                }
            }
        }
        internal static class AccelirateActivityApiTest
        {
            //play around with naming standard
            private const string RuleId = "ACTIVITY_API_TEST"; 

            internal static Rule<IActivityModel> Get()
            {
                var Recommendation = "Testing the Activity Interface";
                var rule = new Rule<IActivityModel>("Activity Interface Test", RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                };
                return rule;
            }

            private static InspectionResult Inspect(IActivityModel activityModel, Rule ruleInstance)
            {
                var messageList = new List<string>();

                //stuff that deals with just the activity
                messageList.Add(string.Format("activityModel.DisplayName: {0}", activityModel.DisplayName));
                messageList.Add(string.Format("activityModel.Type: {0}", activityModel.Type));
                messageList.Add(string.Format("activityModel.Type: {0}", activityModel.Parent.DisplayName));
                messageList.Add(string.Format("activityModel.Parent.DisplayName: {0}", activityModel.Parent.DisplayName));
                messageList.Add(string.Format("activityModel.Context.Project.DisplayName: {0}", activityModel.Context.Project.DisplayName));

                foreach (var arg in activityModel.Arguments)
                {
                    messageList.Add(string.Format("arg.DisplayName: {0}", arg.DisplayName));
                }

                foreach (var child in activityModel.Children)
                {
                    messageList.Add(string.Format("child.DisplayName: {0}", child.DisplayName));
                }

                foreach (var property in activityModel.Properties)
                {
                    messageList.Add(string.Format("property.DisplayName: {0}", property.DisplayName));
                }

                foreach (var variables in activityModel.Variables)
                {
                    messageList.Add(string.Format("variables.DisplayName: {0}", variables.DisplayName));
                }

                foreach (var variables in activityModel.Context.Variables)
                {
                    messageList.Add(string.Format("activityModel.Context.Variables var: {0}", variables.DisplayName));
                }

                if (messageList.Count > 0)
                {
                    return new InspectionResult()
                    {
                        ErrorLevel = ruleInstance.ErrorLevel,
                        HasErrors = true,
                        RecommendationMessage = ruleInstance.RecommendationMessage,
                        Messages = messageList,
                        DocumentationLink = ruleInstance.DocumentationLink
                    };
                }
                else
                {
                    return new InspectionResult() { HasErrors = false };
                }
            }
        }
        internal static class AccelirateWorkflowApiTest
        {
            //play around with naming standard
            private const string RuleId = "WORKFLOW_API_TEST";

            internal static Rule<IWorkflowModel> Get()
            {
                var Recommendation = "Testing the Workflow Interface";
                var rule = new Rule<IWorkflowModel>("Workflow Interface Test", RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                };
                return rule;
            }

            private static InspectionResult Inspect(IWorkflowModel workflowModel, Rule ruleInstance)
            {
                var messageList = new List<string>();
                
                //stuff that deals with just the activity
                messageList.Add(string.Format("workflowModel.DisplayName: {0}",workflowModel.DisplayName));
                messageList.Add(string.Format("workflowModel.RelativePath: {0}", workflowModel.RelativePath));
                messageList.Add(string.Format("workflowModel.Root.DisplayName: {0}", workflowModel.Root.DisplayName));

                foreach(var args in workflowModel.Arguments)
                {
                    messageList.Add(string.Format("workflowModel arg: {0}", args));
                }

                foreach (var args in workflowModel.ImportedNamespaces)
                {
                    messageList.Add(string.Format("imported namespaces: {0}", args));
                }

                if (messageList.Count > 0)
                {
                    return new InspectionResult()
                    {
                        ErrorLevel = ruleInstance.ErrorLevel,
                        HasErrors = true,
                        RecommendationMessage = ruleInstance.RecommendationMessage,
                        Messages = messageList,
                        DocumentationLink = ruleInstance.DocumentationLink
                    };
                }
                else
                {
                    return new InspectionResult() { HasErrors = false };
                }
            }
        }

        public void Initialize(IAnalyzerConfigurationService workflowAnalyzerConfigService)
        {

            //workflowAnalyzerConfigService.AddRule(AccelirateVariableNamingRuleLowerCamelCase.Get());
            //workflowAnalyzerConfigService.AddRule(AccelirateActivityNamingRuleReflectAction.Get());
            //workflowAnalyzerConfigService.AddRule(AccelirateProcessNamingConvention.Get());
            //workflowAnalyzerConfigService.AddRule(AccelirateGeneralNamingConventionActivity.Get());
            //workflowAnalyzerConfigService.AddRule(AccelirateGeneralNamingConventionWorkflow.Get());
            //workflowAnalyzerConfigService.AddRule(AccelirateGeneralNamingConventionProject.Get());
            //workflowAnalyzerConfigService.AddRule(AccelirateProjectApiTest.Get());
            //workflowAnalyzerConfigService.AddRule(AccelirateProjectDirectoryHasConfig.Get());
            //workflowAnalyzerConfigService.AddRule(AccelirateProjectRootDirectoryClean.Get());
            //workflowAnalyzerConfigService.AddRule(AccelirateProjectFolderNames.Get());
            //workflowAnalyzerConfigService.AddRule(AccelirateProjectModuleNames.Get());
            workflowAnalyzerConfigService.AddRule(AccelirateGeneralNamingConventionActivity.Get());
            //workflowAnalyzerConfigService.AddRule(AccelirateParameterExample.Get());

        }
    }

}
