namespace EmployeeManagementSaaS.Application.Queries;

public record GetAllEmployeesQuery() : IRequest<IReadOnlyCollection<EmployeeDto>>;

public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IReadOnlyCollection<EmployeeDto>>
{
    private readonly IEmployeesService _employeesService;

    public GetAllEmployeesQueryHandler(IEmployeesService employeesService)
    {
        _employeesService = employeesService;
    }

    public async Task<IReadOnlyCollection<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        return await _employeesService.GetEmployees();
    }
}
