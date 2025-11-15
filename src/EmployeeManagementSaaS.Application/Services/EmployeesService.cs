namespace EmployeeManagementSaaS.Application.Services;

public class EmployeesService : IEmployeesService
{
    private readonly ILogger<EmployeesService> _logger;
    private readonly IEmployeesRepository _employeeRepository;
    private readonly IMapper _mapper;

    public EmployeesService(ILogger<EmployeesService> logger, IEmployeesRepository employeeRepository, IMapper mapper)
    {
        _mapper = mapper;
        _logger = logger;
        _employeeRepository = employeeRepository;
    }

    public async Task<List<EmployeeDto>> GetEmployees()
    {
        try
        {
            var employees = await _employeeRepository.GetEmployees().ConfigureAwait(false);
            return employees.Select(x => _mapper.Map<EmployeeDto>(x)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving skills");
            return new List<EmployeeDto>();
        }
    }

    public async Task<EmployeeDto?> AssignSkillToEmployee(AssignSkillToEmployeeCommand request)
    {
        try
        {
            var employee = await _employeeRepository.AssignSkillToEmployee(request.EmployeeID, request.SkillID).ConfigureAwait(false);
            return _mapper.Map<EmployeeDto>(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning Skill to Employee");
            return null;
        }
    }
}
