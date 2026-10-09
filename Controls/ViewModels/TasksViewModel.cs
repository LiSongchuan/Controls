using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.ViewModels
{

    /*
     * TasksViewModel (主页面 VM，调度中心)
                    ├── DateSelectorViewModel (顶部日期 VM) -> 控件绑定它
                    ├── ObservableCollection<ScheduleItem> TodaySchedules (左侧数据源) -> 控件绑定它
                    └── ObservableCollection<TodoItem> TodoItems (右侧数据源) -> 控件绑定它
     */
    public partial class TasksViewModel:ObservableObject
    {
        

    }
}
