using Prism.Commands;
using Prism.Mvvm;
using System;
using Microsoft.Win32;
using GenerateTool.Helper;
using System.ComponentModel;
using System.Data;

namespace GenerateTool.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Data Base Generate Tool";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {
            /*Init Command*/
            getInputFile = new DelegateCommand(getInputFileExecute);
            getOutFolder = new DelegateCommand(getOutFolderExecute);
            generate = new DelegateCommand(generateExecute);

            /*Init for BackgroundWorker*/
            _generateWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            _generateWorker.DoWork += generateWorker;
            _generateWorker.ProgressChanged += backgroundworker_PogressChange;
            _generateWorker.RunWorkerCompleted += backgroundWorker_RunComplete;
        }

        private string _inputFile;
        public string inputFile
        {
            get => _inputFile;
            set
            {
                SetProperty(ref _inputFile, value);
                RaisePropertyChanged(nameof(inputFile));
            }
        }

        private string _outFolder;
        public string outFolder
        {
            get => _outFolder;
            set
            {
                SetProperty(ref _outFolder, value);
                RaisePropertyChanged(nameof(outFolder));
            }
        }

        private string _notify;
        public string notify
        {
            get => _notify;
            set
            {
                SetProperty(ref _notify, value);
                RaisePropertyChanged(nameof(notify));
            }
        }

        private string _startSheet;
        public string startSheet
        {
            get => _startSheet;
            set
            {
                SetProperty(ref _startSheet, value);
                RaisePropertyChanged(nameof(startSheet));
            }
        }

        private string _endSheet;
        public string endSheet
        {
            get => _endSheet;
            set
            {
                SetProperty(ref _endSheet, value);
                RaisePropertyChanged(nameof(endSheet));
            }
        }

        private string _sysDate = DateTime.Today.ToString("dd/MM/yyyy");
        public string sysDate
        {
            get => _sysDate;
            set
            {
                SetProperty(ref _sysDate, value);
            }
        }

        private int _progress;
        public int Progress
        {
            get => _progress;
            set
            {
                SetProperty(ref _progress, value);
                RaisePropertyChanged(nameof(Progress));
            }
        }

        private string _nameSpace;
        public string nameSpace
        {
            get => _nameSpace;
            set
            {
                SetProperty(ref _nameSpace, value);
                RaisePropertyChanged(nameof(nameSpace));
            }
        }

        public BackgroundWorker _generateWorker;
        
        public DelegateCommand getInputFile { get; set; }
        public void getInputFileExecute()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog().Equals(true))
            {
                inputFile = dialog.FileName;
            }
        }

        public DelegateCommand getOutFolder { get; set; }
        public void getOutFolderExecute()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    outFolder = dialog.SelectedPath;
                }
            }
        }
        
        public DelegateCommand generate { get; set; }
        public void generateExecute()
        {

            if (this.startSheet.Equals("") || this.endSheet.Equals("")) return;

            if (_generateWorker.IsBusy != true)
            {
                _generateWorker.RunWorkerAsync();
                this.notify = string.Empty;
                this.notify = "Generating database........";
            }
        }
        
        /*Background worker*/
        private void generateWorker(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            if(worker.CancellationPending)
            {
                e.Cancel = true;
            }
            else
            {
                string sheetName = string.Empty;
                int start = int.Parse(startSheet);
                int end = int.Parse(endSheet);
                /*Read All Sheet*/
                for (int sheet = start; sheet < end; sheet++)
                {
                    DataTable table = ExcelHelper.ExcelHelperGetTable(sheet, inputFile, out sheetName);
                    XAFHelper.XAFMakeFile(sheetName, outFolder, out string createdFileName);
                    XAFHelper.XAFWrite(nameSpace, sheetName, createdFileName, outFolder, table);
                    worker.ReportProgress(sheet / (end - start) * 100);
                    this.notify += Environment.NewLine + "Generate sucessfully for " + sheetName + ".cs"; 
                }
            }
        }
        
        private void backgroundworker_PogressChange(object sender, ProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
        }
        
        private void backgroundWorker_RunComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Windows.MessageBox.Show("Generate complete");
        }
    }
}
