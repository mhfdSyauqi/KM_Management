import { ExportRateAndFeedbackToExcel } from '@/api/reqDashboard.js'
import { ref } from 'vue'

const filterExportExcel = ref({
    filter_date:"",
    start_date:"",
    end_date:"",
    rating:"",
    page_limit:null,
    current_page:null,
  })

  async function HandleExcelExport() {
    // Ekspor ke Excel setelah berhasil menambahkan kategori
    const exportResult = await ExportRateAndFeedbackToExcel({
        Filter_Date:filterExportExcel.value.filter_date,
        Start_Date:filterExportExcel.value.start_date,
        End_Date:filterExportExcel.value.end_date,
        Rating:filterExportExcel.value.rating,
        Page_Limit:filterExportExcel.value.page_limit,
        Current_Page:filterExportExcel.value.current_page,
    })
  
    if (!exportResult.is_success) {
      // Handle error saat ekspor ke Excel
      console.error('Error exporting to Excel:', exportResult.error)
      // Anda dapat menambahkan log atau tindakan lain sesuai kebutuhan
    }
  }

  async function HandlePaginationExport(nextPage) {
    filterExportExcel.value.current_page = nextPage
  }
  
  async function HandlingPageLimitExport(limit) {
    filterExportExcel.value.page_limit = limit
  }

  export { filterExportExcel, HandleExcelExport, HandlePaginationExport, HandlingPageLimitExport}