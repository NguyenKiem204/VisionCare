using VisionCare.Application.DTOs.Ehr;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Ehr;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Ehr;

public class PrescriptionService : IPrescriptionService
{
    private readonly IPrescriptionRepository _prescriptionRepository;
    private readonly IEncounterRepository _encounterRepository;

    public PrescriptionService(
        IPrescriptionRepository prescriptionRepository,
        IEncounterRepository encounterRepository
    )
    {
        _prescriptionRepository = prescriptionRepository;
        _encounterRepository = encounterRepository;
    }

    public async Task<PrescriptionDto> CreateAsync(CreatePrescriptionRequest request, int doctorId)
    {
        var e = await _encounterRepository.GetByIdAsync(request.EncounterId);
        if (e == null)
            throw new NotFoundException("Encounter not found");
        if (e.DoctorId != doctorId)
            throw new UnauthorizedAccessException("Cannot create prescription for another doctor's encounter");

        var p = new Prescription
        {
            EncounterId = request.EncounterId,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow,
        };
        var lines = (request.Lines ?? new()).Select(l => new PrescriptionLine
        {
            DrugCode = l.DrugCode,
            DrugName = l.DrugName,
            Dosage = l.Dosage,
            Frequency = l.Frequency,
            Duration = l.Duration,
            Instructions = l.Instructions,
        });
        var created = await _prescriptionRepository.AddAsync(p, lines);
        return Map(created);
    }

    public async Task<IEnumerable<PrescriptionDto>> GetByEncounterAsync(int encounterId, int doctorId)
    {
        var e = await _encounterRepository.GetByIdAsync(encounterId);
        if (e == null)
            throw new NotFoundException("Encounter not found");
        if (e.DoctorId != doctorId)
            throw new UnauthorizedAccessException("Cannot view prescriptions of another doctor's encounter");

        var list = await _prescriptionRepository.GetByEncounterAsync(encounterId);
        return list.Select(Map).ToList();
    }

    private static PrescriptionDto Map(Prescription p)
    {
        return new PrescriptionDto
        {
            Id = p.Id,
            EncounterId = p.EncounterId,
            Notes = p.Notes,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            Lines = p.Lines.Select(l => new PrescriptionLineDto
            {
                Id = l.Id,
                PrescriptionId = l.PrescriptionId,
                DrugCode = l.DrugCode,
                DrugName = l.DrugName,
                Dosage = l.Dosage,
                Frequency = l.Frequency,
                Duration = l.Duration,
                Instructions = l.Instructions,
            }).ToList()
        };
    }
}


