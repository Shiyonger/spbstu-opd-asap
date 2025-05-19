using System.Transactions;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Application.Services;

public class CoursesService(
    ICoursesRepository coursesRepository,
    IAssignmentsRepository assignmentsRepository,
    IStudentRepository studentRepository,
    IGoogleRepository googleRepository) : ICoursesService
{
    private readonly ICoursesRepository _coursesRepository = coursesRepository;
    private readonly IAssignmentsRepository _assignmentsRepository = assignmentsRepository;
    private readonly IStudentRepository _studentRepository = studentRepository;
    private readonly IGoogleRepository _googleRepository = googleRepository;

    public Task<List<Course>> GetByUserId(long userId, CancellationToken ct)
    {
        return _coursesRepository.GetByUserId(userId, ct);
    }

    public async Task<List<Course>> GetForCreateSpreadSheet(CancellationToken ct)
    {
        var courses = await _coursesRepository.GetForCreateSpreadSheet(ct);
        foreach (var course in courses)
        {
            var students = _studentRepository.GetByCourseId(course.Id, ct);
            var assignments = _assignmentsRepository.GetByCourseId(course.Id, ct);
            await Task.WhenAll(students, assignments);

            course.Students = students.Result;
            course.Assignments = assignments.Result;
        }

        return courses;
    }

    public async Task UpdateSpreadSheets(List<Course> courses, CancellationToken ct)
    {
        using var transaction = CreateTransactionScope();

        await _coursesRepository.UpdateSpreadSheet(courses, ct);
        foreach (var course in courses)
        {
            await _assignmentsRepository.UpdateSpreadSheet(course.Assignments!, ct);
            await _googleRepository.CreatePositions(course.Id, course.Students!, course.Assignments!, ct);
        }

        transaction.Complete();
    }

    private static TransactionScope CreateTransactionScope(
        IsolationLevel level = IsolationLevel.ReadCommitted)
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = level,
                Timeout = TimeSpan.FromSeconds(5)
            },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}