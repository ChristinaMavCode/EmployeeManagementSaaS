
namespace EmployeeManagementSaaS.Application.Commands;

public class AssignSkillToEmployeeCommand : IRequest<EmployeeDto?>
{
    public string EmployeeID  { get; set; }
    public string SkillID { get; set; }
}

public class AssignSkillToEmployeeCommandHandler : IRequestHandler<AssignSkillToEmployeeCommand, EmployeeDto?>
{
    private readonly IEmployeesService _service;

    public AssignSkillToEmployeeCommandHandler(IEmployeesService service)
    {
        _service = service;
    }

    public async Task<EmployeeDto?> Handle(AssignSkillToEmployeeCommand request, CancellationToken cancellationToken)
    {
        return await _service.AssignSkillToEmployee(request);
    }
}
