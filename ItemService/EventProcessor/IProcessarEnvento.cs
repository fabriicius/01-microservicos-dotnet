namespace ItemService.EventProcessor
{
   
    public interface IProcessarEnvento
    {
        void Processa (string mensagem);
    }
}