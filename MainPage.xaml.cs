using InnovamedProcedimentos.Models;using InnovamedProcedimentos.Services;
namespace InnovamedProcedimentos;
public partial class MainPage:ContentPage{
 readonly DatabaseService db=new();bool ready;
 public MainPage(){InitializeComponent();}
 protected override async void OnAppearing(){base.OnAppearing();if(ready)return;await Init();ready=true;}
 async Task Init(){try{await db.InitializeAsync();TipoPicker.Items.Add("Todos");foreach(var x in await db.GetTiposAsync())TipoPicker.Items.Add(x);TipoPicker.SelectedIndex=0;await Equip();await Search();StatusLabel.Text=$"{await db.CountAsync():N0} procedimentos • offline";}catch(Exception ex){await DisplayAlert("Erro",ex.Message,"OK");}}
 async void TipoPicker_SelectedIndexChanged(object s,EventArgs e){if(TipoPicker.SelectedIndex>=0)await Equip();}
 async Task Equip(){string? t=TipoPicker.SelectedIndex<=0?null:TipoPicker.SelectedItem?.ToString();EquipamentoPicker.Items.Clear();EquipamentoPicker.Items.Add("Todos");foreach(var x in await db.GetEquipamentosAsync(t))EquipamentoPicker.Items.Add(x);EquipamentoPicker.SelectedIndex=0;}
 async void BuscarButton_Clicked(object s,EventArgs e)=>await Search();async void BuscaEntry_Completed(object s,EventArgs e)=>await Search();
 async Task Search(){string? t=TipoPicker.SelectedIndex<=0?null:TipoPicker.SelectedItem?.ToString();string? eq=EquipamentoPicker.SelectedIndex<=0?null:EquipamentoPicker.SelectedItem?.ToString();var r=await db.PesquisarAsync(t,eq,BuscaEntry.Text);ResultadosCollection.ItemsSource=r;StatusLabel.Text=$"{r.Count:N0} resultado(s)";}
 async void ResultadosCollection_SelectionChanged(object s,SelectionChangedEventArgs e){if(e.CurrentSelection.FirstOrDefault() is Procedimento p){((CollectionView)s).SelectedItem=null;await Navigation.PushAsync(new DetalhePage(p));}}
}