namespace LemonExam.Infrastructure {
    public interface IPostRequestHandler<in TRequest, in TResponse> {
        void Handle(TRequest request, TResponse response);
    }

    public interface IPreRequestHandler<in TRequest> {
        void Handle(TRequest request);
    }
}
