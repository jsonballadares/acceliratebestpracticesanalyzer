using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
                var DocumentationLink = Strings.ST_ACC_002_DocumentationLink;
                var rule = new Rule<IActivityModel>(Strings.ST_ACC_002_Name, RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                    DocumentationLink = DocumentationLink,
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
        internal static class AccelirateVariableNamingRuleReflectAction
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
                var DocumentationLink = Strings.ST_ACC_006_DocumentationLink;
                var rule = new Rule<IProjectModel>(Strings.ST_ACC_006_Name, RuleId, Inspect)
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
            internal static Rule<IActivityModel> Get()
            {
                var Recommendation = Strings.ST_ACC_009_Recomendation;
                var DocumentationLink = Strings.ST_ACC_009_DocumentationLink;
                var rule = new Rule<IActivityModel>(Strings.ST_ACC_009_Name, RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                    DocumentationLink = DocumentationLink,
                };
                return rule;
            }

            private static InspectionResult Inspect(IActivityModel activityModel, Rule ruleInstance)
            {
                var regex = new Regex(alphabeticRegex);
                var messageList = new List<string>();

                foreach (var activityModelVariable in activityModel.Variables)
                {
                    //The name should contain ONLY alphabetic characters. 
                    if (!regex.IsMatch(activityModelVariable.DisplayName))
                    {
                        messageList.Add(string.Format(Strings.ST_ACC_009_ErrorFormatActivityVariableName, activityModelVariable.DisplayName));
                    }

                    //checking if variables they fall within length restraints
                    if (activityModelVariable.DisplayName.Length < 3 || activityModelVariable.DisplayName.Length > 10)
                    {
                        messageList.Add(string.Format(Strings.ST_ACC_009_ErrorFormatActivityVariableLength, activityModelVariable.DisplayName));
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
                var DocumentationLink = Strings.ST_ACC_009_DocumentationLink;
                var rule = new Rule<IWorkflowModel>(Strings.ST_ACC_009_Name, RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                    DocumentationLink = DocumentationLink,
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
                var DocumentationLink = Strings.ST_ACC_009_DocumentationLink;
                var rule = new Rule<IProjectModel>(Strings.ST_ACC_009_Name, RuleId, Inspect)
                {
                    ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                    RecommendationMessage = Recommendation,
                    DocumentationLink = DocumentationLink,
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
        internal static class AccelirateProjectContainsConfig
        {
            //play around with naming standard
            private const string RuleId = "ST_ACC_010";
            //regex for matching ONLY alphabet characters
            private const string alphabeticRegex = @"^[A-Za-z]+$";
            internal static Rule<IProjectModel> Get()
            {
                var DocumentationLink = Strings.ST_ACC_010_DocumentationLink;
                var Recommendation = Strings.ST_ACC_010_Recommendation;
                var rule = new Rule<IProjectModel>(Strings.ST_ACC_010_Name, RuleId, Inspect)
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



                messageList.Add(string.Format("Project DisplayName: {0}", ((IInspectionObject)projectModel).DisplayName));
                messageList.Add(string.Format("Project ProjectProfileType: {0}", projectModel.ProjectProfileType));
                messageList.Add(string.Format("Project ProjectOutputType: {0}", projectModel.ProjectOutputType));
                messageList.Add(string.Format("Project ExpressionLanguage: {0}", projectModel.ExpressionLanguage));
                messageList.Add(string.Format("Project ExceptionHandlerWorkflowName: {0}", projectModel.ExceptionHandlerWorkflowName));
                messageList.Add(string.Format("Project EntryPointName: {0}", projectModel.EntryPointName));
                messageList.Add(string.Format("Project FilePath: {0}", projectModel.ProjectFilePath));

                foreach (var file in projectModel.FileNames)
                {
                    messageList.Add(string.Format("File: {0}", file.ToString()));
                    //if(file.ToString().Split('/')[file.ToString().Split('/').Length - 2].Equals())
                }


                foreach (var workflow in projectModel.Workflows)
                {
                    messageList.Add(string.Format("workflow displayname: {0}", workflow.DisplayName));
                    messageList.Add(string.Format("workflow relativpath: {0}", workflow.RelativePath));
                    messageList.Add(string.Format("workflow root displayma,e: {0}", workflow.Root.DisplayName));
                    
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
            workflowAnalyzerConfigService.AddRule(AccelirateVariableNamingRuleLowerCamelCase.Get());
            workflowAnalyzerConfigService.AddRule(AccelirateVariableNamingRuleReflectAction.Get());
            workflowAnalyzerConfigService.AddRule(AccelirateProcessNamingConvention.Get());
            workflowAnalyzerConfigService.AddRule(AccelirateGeneralNamingConventionActivity.Get());
            workflowAnalyzerConfigService.AddRule(AccelirateGeneralNamingConventionWorkflow.Get());
            workflowAnalyzerConfigService.AddRule(AccelirateGeneralNamingConventionProject.Get());
            //workflowAnalyzerConfigService.AddRule(AccelirateProjectContainsConfig.Get());
        }
    }

}
