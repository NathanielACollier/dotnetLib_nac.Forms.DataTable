using System.Collections.ObjectModel;

namespace TestApp.model
{
    public class TestDataContextWindowModel : nac.Forms.model.ViewModelBase
    {

        public ObservableCollection<model.Alphabet> Letters
        {
            get { return GetValue(() => Letters); }
            set{ SetValue(() => Letters, value);}
        }

        public model.Alphabet NewLetter
        {
            get { return GetValue(() => NewLetter); }
            set { SetValue(() => NewLetter, value);}
        }

        public TestDataContextWindowModel()
        {
            this.Letters = new ObservableCollection<Alphabet>();
            this.NewLetter = new model.Alphabet();
        }
    }
}