using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MMPI
{
  public class AnalyzerViewModel
  {
    private List<ResultViewModel> _Results;

    private UserViewModel _User;

    private FlowDocument _Document;
    public AnalyzerViewModel( List<ResultViewModel> results,UserViewModel user )
    {
      _Results = results;
      _User = user;
      _Document = new FlowDocument();
    }

    private void FillHeader()
    {

    }

  }
}
