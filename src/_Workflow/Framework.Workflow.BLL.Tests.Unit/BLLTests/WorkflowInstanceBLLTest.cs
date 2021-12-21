//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Framework.Authorization.Domain;
//using Framework.Transfering;
//using Framework.Workflow.BLL;
//using Framework.Workflow.Domain;
//using Framework.Workflow.Domain.Definition;
//using Framework.Workflow.Domain.Runtime;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NSubstitute;

//namespace Framework.Workflow.BLL.Tests.Unit
//{
//    [TestClass]
//    public class WorkflowInstanceBLLTest : BaseUnitTest
//    {
//        protected IWorkflowBLLContext _context;
//        protected Domain.Definition.Workflow _workflow;
//        protected WorkflowInstance _workflowInstance;

//        private const string testPrincipal = "TestPrincip";

//        private void ActAsSomePrincipal(string principal)
//        {
//            _context.Authorization.RunAsManager.PrincipalName.Returns(principal);
//            _context.Authorization.CurrentPrincipalName.Returns(principal);
//        }

//        protected Domain.Definition.Workflow CreateTestWorkflow()
//        {
//            var workflow = _context.Logics.Workflow.Create(new WorkflowCreateModel());

//            var principal = new Principal() { Name = testPrincipal };
//            var permission = new Permission(principal);
//            var bRole = new BusinessRole() { Name = "TestBusinessRole" };
//            permission.Role = bRole;
//            _context.Authorization.Logics.BusinessRole.Save(bRole);
//            _context.Authorization.Logics.Principal.Save(principal);
//            _context.Authorization.Logics.Permission.Save(permission);

//            var operation = new Operation() { Name = "TestOperMain" };
//            var link = new BusinessRoleOperationLink(bRole);
//            link.Operation = operation;
//            _context.Authorization.Logics.Operation.Save(operation);

//            var roleAdded = new Role(workflow) { Name = "Comm1Role" };
//            roleAdded.SecurityOperationId = operation.Id;
//            _context.Logics.Role.Save(roleAdded);
//            //var defRole = new Role()

//            workflow.Name = "WFTest1";

//            workflow.TargetSystem = new TargetSystem()
//                                    {
//                                        Name = "WFTest1_Target"
//                                    };
//            //States
//            var stateNew = new State(workflow);
//            stateNew.Name = "New";
//            stateNew.IsInitial = true;
//            //stateNew.Activities.First().Name = "New";

//            var stateIP = new State(workflow);
//            stateIP.Name = "InProgress";

//            var stateReject = new State(workflow);
//            stateReject.Name = "Rejected";

//            var stateFinal = new State(workflow);
//            stateFinal.Name = "Final";
//            stateFinal.IsFinal = true;

//            _context.Logics.State.Save(stateNew);
//            _context.Logics.State.Save(stateIP);
//            _context.Logics.State.Save(stateReject);
//            _context.Logics.State.Save(stateFinal);

//            //Transition
//            var transNewIP = new Transition(workflow);
//            transNewIP.Name = "NewToIP";
//            transNewIP.From = stateNew;
//            transNewIP.To = stateIP;

//            var transNewReject = new Transition(workflow);
//            transNewReject.Name = "NewToReject";
//            transNewReject.From = stateNew;
//            transNewReject.To = stateReject;

//            var transIPFinal = new Transition(workflow);
//            transIPFinal.Name = "IPToFinal";
//            transIPFinal.From = stateIP;
//            transIPFinal.To = stateFinal;

//            _context.Logics.Transition.Save(transNewIP);
//            _context.Logics.Transition.Save(transNewReject);
//            _context.Logics.Transition.Save(transIPFinal);

//            //Tasks
//            var tsk1 = new Task(stateNew);
//            tsk1.Name = "tsk1";

//            var tsk2 = new Task(stateIP);
//            tsk2.Name = "tsk2";

//            _context.Logics.Task.Save(tsk1);
//            _context.Logics.Task.Save(tsk2);


//            //Commands
//            var comm1 = new Command(tsk1);
//            comm1.Name = "CommandFromNew";

//            //Commands
//            var comm2 = new Command(tsk2);
//            comm2.Name = "CommandFromInProgress";

//            var commAcc1 = new CommandRoleAccess(comm1);
//            commAcc1.Role = roleAdded;

//            var commAcc2 = new CommandRoleAccess(comm2);
//            commAcc2.Role = roleAdded;

//            _context.Logics.Default.Create<CommandRoleAccess>().Save(commAcc1);
//            _context.Logics.Default.Create<CommandRoleAccess>().Save(commAcc2);

//            _context.Logics.Command.Save(comm1);
//            _context.Logics.Command.Save(comm2);

//            var comm1Event = new CommandExecutionEvent(comm1);
//            comm1Event.Name = "command1Event";

//            var comm2Event = new CommandExecutionEvent(comm2);
//            comm2Event.Name = "command2Event";

//            _context.Logics.CommandExecutionEvent.Save(comm1Event);
//            _context.Logics.CommandExecutionEvent.Save(comm2Event);


//            var eventTransition = new EventTransition(workflow);
//            eventTransition.Name = "EventTransition1";
//            eventTransition.TriggerEvent = comm1.Event;
//            eventTransition.From = stateNew;
//            eventTransition.To = stateIP;

//            var eventTransition2 = new EventTransition(workflow);
//            eventTransition2.Name = "EventTransition2";
//            eventTransition2.TriggerEvent = comm2.Event;
//            eventTransition2.From = stateIP;
//            eventTransition2.To = stateFinal;

//            _context.Logics.EventTransition.Save(eventTransition);
//            _context.Logics.EventTransition.Save(eventTransition2);

//            _context.Logics.Workflow.Save(workflow);

//            return workflow;
//        }

//        public WorkflowInstance CreateTestWorkflowInstance(Domain.Definition.Workflow workflow)
//        {
//            var context = GetContext();
//            var wfinstance = context.Logics.WorkflowInstance.Start(new StartWorkflowRequest()
//                {
//                    Workflow = workflow,
//                    Parameters = new List<StartWorkflowRequestParameter>()
//                        {
//                            new StartWorkflowRequestParameter()
//                                {
//                                    Value = Guid.Empty.ToString(),
//                                    Definition = new WorkflowParameter(workflow)
//                                        {
//                                            Name = "WFParam1",
//                                            Type = new ParameterType(workflow.TargetSystem, ParameterTypeRole.Domain) { Name = "WorkflowInstanceBLL"},
//                                            StorageType = StorageType.Workflow,
//                                            AllowNull = true
//                                        }
//                                }
//                        }
//                });

//            return wfinstance;
//        }


//        [TestInitialize]
//        public void Initialize()
//        {
//            _context = GetContext();
//            _workflow = CreateTestWorkflow();
//            _workflowInstance = CreateTestWorkflowInstance(_workflow);
//        }

//        [TestCleanup]
//        public void Cleanup()
//        {
//            _context = null;
//            _workflow = null;
//            _workflowInstance = null;
//        }

//        [TestMethod]
//        public void CreateTestWorkflowTest()
//        {
//            Assert.IsNotNull(_workflow);
//        }

//        [TestMethod]
//        public void CreateTestWorkflowInstanceTest()
//        {
//            Assert.IsNotNull(_workflowInstance);
//        }

//        [Ignore]
//        [TestMethod]
//        public void ExecuteCommandTest_FirstCommand()
//        {
//            ActAsSomePrincipal(testPrincipal);

//            var result = _context.Logics.WorkflowInstance.ExecuteCommand(new ExecuteCommandRequest()
//                {
//                    WorkflowInstance = _workflowInstance,
//                    Command = _workflow.States.First().Tasks.First().Commands.First()
//                });

//            Assert.IsNotNull(result);
//        }

//        [Ignore]
//        [TestMethod]
//        public void ExecuteCommandTest_EachCommand()
//        {
//            ActAsSomePrincipal(testPrincipal);

//            foreach (var state in _workflow.States)
//            {
//                if (state.Tasks.Any() && state.Tasks.First().Commands.Any())
//                {
//                    var result = _context.Logics.WorkflowInstance.ExecuteCommand(new ExecuteCommandRequest()
//                        {
//                            WorkflowInstance = _workflowInstance,
//                            Command = state.Tasks.First().Commands.First()
//                        });

//                    Assert.IsNotNull(result);
//                }
//            }
//        }
//        [Ignore]
//        [TestMethod]
//        public void ExecuteCommandTest_FirstCommand_WithParameter()
//        {
//            ActAsSomePrincipal(testPrincipal);

//            var command = _workflow.States.First().Tasks.First().Commands.First();

//            var definition = new CommandParameter(command)
//                {
//                    Name = "CommandPar1",
//                    Type = new ParameterType(_workflow.TargetSystem, ParameterTypeRole.Domain) { Name = "WorkflowInstanceBLL" },
//                    AllowNull = true,
//                    StorageType = StorageType.Command
//                };

//            var result = _context.Logics.WorkflowInstance.ExecuteCommand(new ExecuteCommandRequest()
//            {
//                WorkflowInstance = _workflowInstance,
//                Command = command,
//                Parameters = new List<ExecuteCommandRequestParameter>
//                { new ExecuteCommandRequestParameter()
//                    {
//                        Definition = definition,
//                        Value = Guid.Empty.ToString()
//                    }
//                }
//            });

//            Assert.IsNotNull(result);
//        }
//    }
//}
