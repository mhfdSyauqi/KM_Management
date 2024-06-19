import { ExportCategoryLayerOneAsync } from '@/api/reqCategory.js'
import { ref } from 'vue'

const filterExportCategories = ref(null)

  async function HandleExcelExport() {
    // Ekspor ke Excel setelah berhasil menambahkan kategori
    const exportResult = await ExportCategoryLayerOneAsync({
      Is_Active: filterExportCategories.value // Kirim data kategori yang baru ditambahkan
    })
  
    if (!exportResult.is_success) {
      // Handle error saat ekspor ke Excel
      console.error('Error exporting to Excel:', exportResult.error)
      // Anda dapat menambahkan log atau tindakan lain sesuai kebutuhan
    }
  }

  export { filterExportCategories, HandleExcelExport}