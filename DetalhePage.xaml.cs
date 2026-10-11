using InnovamedProcedimentos.Models;
namespace InnovamedProcedimentos;
public partial class DetalhePage:ContentPage{
 public DetalhePage(Procedimento p){InitializeComponent();Numero.Text=$"Nº {p.numero}";Descricao.Text=p.texto_original;Tipo.Text=p.TipoProcedimento;Equip.Text=p.TipoEquipamento;Info.Text=p.demais_informacoes??"-";Versao.Text=p.versao??"-";Data.Text=p.data_publicacao??"-";}
}