using Microsoft.Win32.TaskScheduler;

TaskDefinition tdOff = TaskService.Instance.NewTask();
tdOff.RegistrationInfo.Description = "Turns dGPU off when on battery";
EventTrigger eTriggerBattery = new EventTrigger();
eTriggerBattery.Subscription = @"<QueryList><Query Id='0' Path='System'><Select Path='System'>*[EventData[Data[@Name='AcOnline']='false']]</Select></Query></QueryList>";
eTriggerBattery.ValueQueries.Add("Name", "Value");
tdOff.Triggers.Add(eTriggerBattery);

tdOff.Actions.Add(new ExecAction(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe",arguments:string.Format("pnputil /disable-device \"{0}\"", "PCI\\VEN_1002'&'DEV_73EF'&'SUBSYS_1DCC1043'&'REV_C0\\6'&'E1D2937'&'0'&'00000009")+";"+"powercfg /setactive 63c6ce89-e91a-4e48-8620-185e7d229f16"));
tdOff.Settings.StopIfGoingOnBatteries = false;
tdOff.Settings.DisallowStartIfOnBatteries = false;
TaskService.Instance.RootFolder.RegisterTaskDefinition("OnBatteryGPU", tdOff);

TaskDefinition tdOn = TaskService.Instance.NewTask();
tdOn.RegistrationInfo.Description = "Turns dGPU on when on AC";
EventTrigger eTriggerAC = new EventTrigger();
eTriggerAC.Subscription = @"<QueryList><Query Id='0' Path='System'><Select Path='System'>*[EventData[Data[@Name='AcOnline']='true']]</Select></Query></QueryList>";
eTriggerAC.ValueQueries.Add("Name", "Value");
tdOn.Triggers.Add(eTriggerAC);
tdOn.Actions.Add(new ExecAction(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe",arguments:string.Format("pnputil /enable-device \"{0}\"", "PCI\\VEN_1002'&'DEV_73EF'&'SUBSYS_1DCC1043'&'REV_C0\\6'&'E1D2937'&'0'&'00000009")+";"+"powercfg /setactive 381b4222-f694-41f0-9685-ff5bb260df2e"));
tdOn.Settings.StopIfGoingOnBatteries = false;
tdOn.Settings.DisallowStartIfOnBatteries = false;
TaskService.Instance.RootFolder.RegisterTaskDefinition("OnACGPU", tdOn);