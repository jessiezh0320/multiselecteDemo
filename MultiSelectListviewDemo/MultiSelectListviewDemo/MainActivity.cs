using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.V7.View;
using Android.Views;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using System.Collections.Generic;
using Android.Util;
using ActionMode = Android.Views.ActionMode;

namespace MultiSelectListviewDemo
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    { //Note：  This demo is based on : https://sabitlabscode.wordpress.com/2014/07/03/xamarin-android-multi-select-with-contextual-action-bar-cab/

        MyAdapter myAdapter;
        ListView myListView;

        LinearLayout below_layout;
        TextView btn_mover;
        TextView btn_lido;
        TextView btn_apagar;

        private ActionMode mActionMode;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            below_layout = FindViewById<LinearLayout>(Resource.Id.below_layout);
            btn_mover = FindViewById<TextView>(Resource.Id.btn_mover);
            btn_lido = FindViewById<TextView>(Resource.Id.btn_lido);
            btn_apagar = FindViewById<TextView>(Resource.Id.btn_apagar);

            btn_mover.Click += Btn_mover_Click;
            btn_lido.Click += Btn_lido_Click;
            btn_apagar.Click += Btn_apagar_Click;

            myAdapter = new MyAdapter(this);//definemyadapter constructor
            myListView = FindViewById<ListView>(Resource.Id.mylistview);//definelistviewwithlistviewinthe xml
            myListView.Adapter = myAdapter;//setlistviewadapterwith myadapter

            this.myListView.ItemLongClick += (sender, e) => {
                //whenyoudolongclickontheitem,itwillrunthis action
                //wewillmakeit later
                OnListItemSelect(e.Position);
            };

            this.myListView.ChoiceMode = ChoiceMode.Multiple;
        }

        private void Btn_apagar_Click(object sender, System.EventArgs e)
        {
            Toast.MakeText(this, "click button apagar......", ToastLength.Long).Show();
        }

        private void Btn_lido_Click(object sender, System.EventArgs e)
        {
            Toast.MakeText(this, "click button lido......", ToastLength.Long).Show();
        }

        private void Btn_mover_Click(object sender, System.EventArgs e)
        {
            Toast.MakeText(this, "click button mover......",ToastLength.Long).Show();
        }

        private void OnListItemSelect(int position)
        {
            myAdapter.toggleSelection(position);
            bool hasCheckedItems = myAdapter.getSelectedCount() > 0;

            if (hasCheckedItems && mActionMode == null)
            {
                mActionMode = StartActionMode(new ActionModeCallback(this));

                below_layout.Visibility = ViewStates.Visible;// display the menu bar
            }
            else if (!hasCheckedItems && mActionMode != null) {
                mActionMode.Finish();
                below_layout.Visibility = ViewStates.Gone;// display the menu bar
            }

            if (mActionMode != null) {
                mActionMode.Title = (myAdapter.getSelectedCount().ToString() + "selected");

                below_layout.Visibility = ViewStates.Visible;// display the menu bar
            }


        }


        private class ActionModeCallback : Java.Lang.Object, ActionMode.ICallback
        {
            MainActivity parent;

            public ActionModeCallback(MainActivity parent)
            {
                this.parent = parent;
            }

            public bool OnCreateActionMode(ActionMode mode, IMenu menu)
            {
                //wewillcallcab_menu.xmlforsetthe menu
                //wewillcreatecab_menu.xml later
                mode.MenuInflater.Inflate(Resource.Layout.cab_menu, menu);
                return true;
            }

            public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
            {
                return false;
            }

            public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
            {
                switch (item.ItemId)
                {
                    //ifuserclickmenu print_me
                    case Resource.Id.print_me:
                        //createanalert dialog
                        AlertDialog.Builder builder = new AlertDialog.Builder(parent);
                        string msg = "Are you sure you want to print items?";
                        builder.SetMessage(msg)
                          .SetCancelable(false)
                          .SetPositiveButton("Yes", (or, er) =>
                          {

                              SparseBooleanArray selected = parent.myAdapter.getSelectedIds();
                              List<string> list_item = new List<string>();
                              for (int i = (selected.Size() - 1); i >= 0; i--)
                              {
                                  //checkisvaluecheckedby user
                                  if (selected.ValueAt(i))
                                  {
                                      int selectedItem = (int)parent.myAdapter.GetItem(selected.KeyAt(i));
                                      list_item.Add(selectedItem.ToString());
                                  }
                              }
                              //print message
                              Toast.MakeText(parent, "YouSelect" + parent.myAdapter.getSelectedCount() + "Item:[" + string.Join(",", list_item) + "]", ToastLength.Long).Show();
                              mode.Finish();

                              parent.below_layout.Visibility = ViewStates.Gone;// Gone

                          })
                        .SetNegativeButton("No", (or, er) =>
                        {
                            parent.below_layout.Visibility = ViewStates.Gone;  // Gone

                            ((Dialog)or).Cancel();
                        });

                        AlertDialog alert = builder.Create();
                        alert.Show();

                        return true;
                    default:
                        return false;
                }
            }

            public void OnDestroyActionMode(ActionMode mode)
            {
                parent.myAdapter.removeSelection();
                parent.mActionMode = null;
            }
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}