namespace InnovamedProcedimentos.Models;
public class Procedimento {
 public int id{get;set;} public string numero{get;set;}="";
 public int tipo_procedimento_id{get;set;} public int equipamento_id{get;set;}
 public string? demais_informacoes{get;set;} public string? versao{get;set;}
 public string? data_publicacao{get;set;} public string? equipamento_busca{get;set;}
 public string? texto_original{get;set;} public string? texto_busca{get;set;} public int ativo{get;set;}
 public string TipoProcedimento{get;set;}=""; public string TipoEquipamento{get;set;}="";
}