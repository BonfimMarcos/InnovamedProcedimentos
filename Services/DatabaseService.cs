using SQLite;
using InnovamedProcedimentos.Models;
namespace InnovamedProcedimentos.Services;
public class DatabaseService {
 SQLiteAsyncConnection? db;
 readonly string path=Path.Combine(FileSystem.AppDataDirectory,"procedimentos_innovamed.db");
 public async Task InitializeAsync(){
  if(db!=null)return;
  if(!File.Exists(path)){await using var s=await FileSystem.Current.OpenAppPackageFileAsync("procedimentos_innovamed.db");await using var d=File.Create(path);await s.CopyToAsync(d);}
  db=new SQLiteAsyncConnection(path);
 }
 SQLiteAsyncConnection D=>db??throw new InvalidOperationException("Banco não inicializado.");
 public Task<int> CountAsync()=>D.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM procedimentos WHERE ativo=1");
 public async Task<List<string>> GetTiposAsync(){
  var r=await D.QueryAsync<ValueRow>("SELECT descricao AS Valor FROM tipos_procedimento WHERE ativo=1 ORDER BY descricao");
  return r.Select(x=>x.Valor!).ToList();
 }
 public async Task<List<string>> GetEquipamentosAsync(string? tipo){
  string sql="SELECT descricao AS Valor FROM equipamentos WHERE ativo=1";var a=new List<object>();
  if(!string.IsNullOrWhiteSpace(tipo)){sql+=" AND id IN (SELECT p.equipamento_id FROM procedimentos p JOIN tipos_procedimento t ON t.id=p.tipo_procedimento_id WHERE t.descricao=?)";a.Add(tipo);}
  sql+=" ORDER BY descricao";
  var r=await D.QueryAsync<ValueRow>(sql,a.ToArray());return r.Select(x=>x.Valor!).ToList();
 }
 public async Task<List<Procedimento>> PesquisarAsync(string? tipo,string? equipamento,string? texto){
  string sql="SELECT p.id,p.numero,p.tipo_procedimento_id,p.equipamento_id,p.demais_informacoes,p.versao,p.data_publicacao,p.equipamento_busca,p.texto_original,p.texto_busca,p.ativo,t.descricao AS TipoProcedimento,e.descricao AS TipoEquipamento FROM procedimentos p JOIN tipos_procedimento t ON t.id=p.tipo_procedimento_id JOIN equipamentos e ON e.id=p.equipamento_id WHERE p.ativo=1";
  var a=new List<object>();
  if(!string.IsNullOrWhiteSpace(tipo)){sql+=" AND t.descricao=?";a.Add(tipo);}
  if(!string.IsNullOrWhiteSpace(equipamento)){sql+=" AND e.descricao=?";a.Add(equipamento);}
  foreach(var termo in (texto??"").Trim().Split(' ',StringSplitOptions.RemoveEmptyEntries)){
   sql+=" AND (p.texto_busca LIKE ? OR p.numero LIKE ? OR p.demais_informacoes LIKE ?)";
   var q="%"+termo.ToLowerInvariant()+"%";a.Add(q);a.Add(q);a.Add(q);
  }
  sql+=" ORDER BY CAST(p.numero AS INTEGER),p.numero LIMIT 500";
  return await D.QueryAsync<Procedimento>(sql,a.ToArray());
 }
 class ValueRow{public string? Valor{get;set;}}
}