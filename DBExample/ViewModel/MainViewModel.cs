using System.Collections.ObjectModel;

namespace DBExample.ViewModel;
public partial class MainViewModel : ObservableObject
{
    UserManagement MyUserTables;

    [ObservableProperty]
    bool isNotBusy=true;

    public ObservableCollection<UserProfile> MyShownList { get; set; } = new();
    public MainViewModel()
    {
        this.MyUserTables = new();
        MyUserTables.ConfigTools();
    }
    [RelayCommand]
    internal async Task FillAccessTable()
    {
        IsNotBusy = false;

        Globals.UserSets.Tables["Access"].Clear();

        try
        {
            await MyUserTables.ReadAccessTable();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Database", ex.Message, "OK");
        }

        IsNotBusy = true;
    }

    [RelayCommand]
    internal async Task FillUserTable()
    {
        IsNotBusy = false;

        Globals.UserSets.Tables["Users"].Clear();
        Globals.UserSets.Tables["Access"].Clear();

        await MyUserTables.FillUsers();

        await MoveIntoList();

        IsNotBusy = true;
    }
    [RelayCommand]
    internal async Task UpdateTable()
    {
        IsNotBusy = false;

        DataRow User1 = Globals.UserSets.Tables["Users"].NewRow();
        User1[1] = "aaeztza";
        User1[2] = "aaa";
        User1[3] = 1;

        DataRow User2 = Globals.UserSets.Tables["Users"].NewRow();
        User2[1] = "bbgsdgs";
        User2[2] = "bbb";
        User2[3] = 2;

        Globals.UserSets.Tables["Users"].Rows.Add(User1);
        Globals.UserSets.Tables["Users"].Rows.Add(User2);

        await MyUserTables.UpdateUsers();

        await MoveIntoList();

        IsNotBusy = true;
    }
    [RelayCommand]
    internal async Task InsertDB()
    {
        IsNotBusy = false;
        await MyUserTables.InsertUsers("Fred","abab",1);
        await MoveIntoList();
        IsNotBusy = true;
    }
    [RelayCommand]
    internal async Task DeleteDB()
    {
        IsNotBusy = false;
        await MyUserTables.DeleteUsers("Fred");
        await MoveIntoList();
        IsNotBusy = true;
    }
    internal async Task MoveIntoList()
    {
        List<UserProfile> MyList = new();

        try
        {            
            MyList = Globals.UserSets.Tables["Users"].AsEnumerable().Select(m => new UserProfile()
            {
                Id = m.Field<Int16>("User_ID"),
                Name = m.Field<string>("UserName"),
                Password = m.Field<string>("UserPassword"),
                AccessType = m.Field<Int16>("UserAccessType"),
            }).ToList();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Database", ex.Message, "OK");
        }
        MyShownList.Clear();

        foreach (UserProfile item in MyList)
        {
            MyShownList.Add(item);
        }
    }
}

