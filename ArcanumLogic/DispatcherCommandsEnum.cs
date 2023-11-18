using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic
{
    public enum DispatcherCommandsEnum
    {
        status,
        start, 
        login_vip,

        init_all,
        init_command,
        init_users,
        init_users_sure_yes,
        init_imagine,
        init_imagine_sure_yes,
        init_tree,
        init_tree_sure_yes,
        init_fabricas,
        update_users,
        update_imagines,
        admin_all_schemes,

        start_cycle,
        stop_cycle,
        calculate,

        schemes,
        learn, 
        learn_neutral, 
        transfer_destiny,
        bid,
        imagines
    }
}
