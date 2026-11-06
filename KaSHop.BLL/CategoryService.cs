
using KaSHop.BLL.Settings;
using KaSHop.DAL.DTO.Requet;
using KaSHop.DAL.DTO.Responses;
using KaSHop.DAL.models;
using KaSHop.DAL.Repositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KaSHop.BLL
{
    public class CategoryService : ICategoryService
    {
        // ============================================
        // Dependency Injection - حقن الاعتمادية
        // Repository responsible for database operations
        // ============================================

        private readonly IGenericRepository<Category> _repo;

        public CategoryService(IGenericRepository<Category> repo)
        {
            _repo = repo;
        }

        // ============================================
        // OLD CREATE METHOD - الدالة القديمة للإضافة
        // Kept as a comment for reference
        // ============================================

        /*
        public async Task<bool> CreateAsync(
            CategoryRequest request,
            CancellationToken cancellationToken = default)
        {
            // Check if category name already exists
            var categories = await _repo.GetAllAsync(cancellationToken);

            var exists = categories.Any(c =>
                c.Name.ToLower() == request.Name.ToLower());

            if (exists)
                return false;

            var category = request.Adapt<Category>();

            await _repo.CreateAsync(category, cancellationToken);

            return true;
        }*/

        // ============================================
        // CREATE CATEGORY - إضافة تصنيف جديد
        // ============================================
        // 1. Validate translations - التحقق من الترجمات
        // 2. Extract translated names - استخراج الأسماء
        // 3. Check duplicates - منع تكرار الأسماء
        // 4. Map DTO to Entity - تحويل البيانات
        // 5. Save category - حفظ التصنيف
        // ============================================

        public async Task<bool> CreateAsync(
            CategoryRequest request,
            CancellationToken cancellationToken = default)
        {
            // التحقق من صحة اللغات والترجمات
            if (!AreTranslationsValid(request.Translations))
                return false;

            // استخراج أسماء التصنيف وتحويلها لأحرف صغيرة
            // لتسهيل المقارنة دون حساسية لحالة الأحرف
            var names = request.Translations
                .Select(t => t.Name.ToLower())
                .ToList();

            // البحث عن تصنيف يحتوي على اسم موجود مسبقًا
            // Any checks translated names in the database
            var existing = await _repo.GetOneAsync(
                filter: c => c.Translations.Any(
                    t => names.Contains(t.Name.ToLower()))
            );

            // منع إضافة تصنيف مكرر
            if (existing is not null)
                return false;

            // تحويل CategoryRequest إلى Category باستخدام Mapster
            var category = request.Adapt<Category>();

            // حفظ التصنيف مع ترجماته في قاعدة البيانات
            await _repo.CreateAsync(category, cancellationToken);

            return true;
        }

        // ============================================
        // DELETE CATEGORY - حذف تصنيف
        // ============================================
        // Find category by ID
        // Return false if not found
        // Delete category if found
        // ============================================

        public async Task<bool> DeleteAsync(
            int Id,
            CancellationToken cancellationToken = default)
        {
            // البحث عن التصنيف باستخدام المعرف
            var category = await _repo.GetOneAsync(
                filter: c => c.Id == Id,
                cancellationToken: cancellationToken
            );

            // إذا لم يكن التصنيف موجودًا لا تتم عملية الحذف
            if (category is null)
                return false;

            // حذف التصنيف من قاعدة البيانات
            await _repo.RemoveAsync(category, cancellationToken);

            return true;
        }

        // ============================================
        // GET ALL CATEGORIES - جلب جميع التصنيفات
        // ============================================
        // Include translations using Eager Loading
        // Convert entities into response DTOs
        // ============================================

        public async Task<List<CategoryResponse>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            // جلب التصنيفات مع الترجمات المرتبطة بها
            // Include loads the Translations navigation property
            var categories = await _repo.GetAllAsync(
                includes: [c => c.Translations],
                cancellationToken: cancellationToken
            );

            // تحويل قائمة التصنيفات إلى CategoryResponse
            // باستخدام Mapster
            return categories.Adapt<List<CategoryResponse>>();
        }

        // ============================================
        // GET CATEGORY BY ID - جلب تصنيف حسب المعرف
        // ============================================
        // Filter by ID and include translations
        // Return null when category does not exist
        // ============================================

        public async Task<CategoryResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            // البحث عن تصنيف محدد باستخدام Id
            // مع تحميل جميع الترجمات الخاصة به
            var category = await _repo.GetOneAsync(
                filter: c => c.Id == id,
                includes: [c => c.Translations],
                cancellationToken: cancellationToken
            );

            // تحويل التصنيف إلى Response
            // ?. prevents mapping when category is null
            return category?.Adapt<CategoryResponse>();
        }

        // ============================================
        // UPDATE CATEGORY - تعديل تصنيف
        // ============================================
        // 1. Find existing category
        // 2. Retrieve categories
        // 3. Map updated data
        // 4. Save changes
        // ============================================
        public async Task<bool> UpdateAsync(
            int Id,
            CategoryRequest request,
            CancellationToken cancellationToken = default)
        {
            if (!AreTranslationsValid(request.Translations))
                return false;

            var category = await _repo.GetOneAsync(
                filter: c => c.Id == Id,
                includes: [c => c.Translations],
                tracked: true,
                cancellationToken: cancellationToken
            );

            if (category is null)
                return false;

            category.Status = request.Status;

            foreach (var translation in request.Translations)
            {
                var existing = category.Translations
                    .FirstOrDefault(t => t.Language == translation.Language);

                if (existing is not null)
                {
                    existing.Name = translation.Name;
                }
                else
                {
                    category.Translations.Add(new CategoryTranslation
                    {
                        Name = translation.Name,
                        Language = translation.Language
                    });
                }
            }

            await _repo.UpdateAsync(category, cancellationToken);

            return true;
        }

        // ============================================
        // VALIDATE TRANSLATIONS - التحقق من الترجمات
        // ============================================
        // Ensure each language appears only once
        // Ensure all supported languages are provided
        // ============================================

        private static bool AreTranslationsValid(
            List<CategoryTranslationRequest> translations)
        {
            // مثال على الترجمات المطلوبة:
            // [{name:'mobile', language:'en'}, {name:'موبايل', language:'ar'}]

            // استخراج رموز اللغات من قائمة الترجمات
            var languages = translations.Select(x => x.Language).ToList();

            //en=en
            // منع تكرار نفس اللغة داخل التصنيف
            if (languages.Distinct().Count() != languages.Count)
                return false;

            // التأكد من أن عدد اللغات يساوي عدد اللغات المدعومة
            // وأن جميع اللغات المطلوبة موجودة
            return languages.Count == SupportedLanguages.Languages.Length
                && SupportedLanguages.Languages.All(lang => languages.Contains(lang));
        }
    }
}
