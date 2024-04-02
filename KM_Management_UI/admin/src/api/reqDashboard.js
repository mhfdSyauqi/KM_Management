import { useRequest } from '@/api/base/useRequest.js'

export async function GetRateAndFeedbackAsync(
  filter_date,
  start_date,
  end_date,
  rating,
  page_limit,
  current_page,
) {
  const Payload = JSON.stringify({
    Filter_Date:filter_date,
    Start_Date:start_date,
    End_Date:end_date,
    Rating:rating,
    Page_limit:page_limit,
    Current_Page:current_page,
  })

  const result = {
    is_success: true,
    rate_and_feedback: [],
    error: null
  }

  await useRequest
    .post('RateAndFeedback/GetRateAndFeedback', Payload)
    .then((res) => {
      result.rate_and_feedback = res.data.data.rate_and_feedback
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}


export async function ExportRateAndFeedbackToExcel(request) {
  const result = {
    is_success: true,
    error: null,
    excelData: null // Menyimpan data excel yang diterima
  };

  await useRequest
    .post(`RateAndFeedback/ExportRateAndFeedbackToExcel`, request, {
      responseType: 'blob' // Mengatur tipe respons sebagai blob (binary data)
    })
    .then((res) => {
      const blob = new Blob([res.data], { type: res.headers['content-type'] }); // Membuat blob dari data respons
      const url = window.URL.createObjectURL(blob); // Membuat URL dari blob

      // Format tanggal dan waktu saat ini
      const dateTimeNow = new Date();
      const formattedDateTime = `${dateTimeNow.getDate()}-${dateTimeNow.getMonth() + 1}-${dateTimeNow.getFullYear()}.${dateTimeNow.getHours()}.${dateTimeNow.getMinutes()}.${dateTimeNow.getSeconds()}`;

      // Misalnya, Anda ingin menampilkan tautan untuk mengunduh file excel
      const downloadLink = document.createElement('a');
      downloadLink.href = url;
      downloadLink.download = `Rate_&_Feedback_${formattedDateTime}.xlsx`; // Nama file excel yang diunduh
      downloadLink.click();

      // Atau, jika Anda ingin menyimpan data excel ke dalam variabel
      result.excelData = blob;

      // Jangan lupa untuk melepaskan objek URL setelah penggunaan
      window.URL.revokeObjectURL(url);
    })
    .catch((err) => {
      result.is_success = false;
      result.error = err.response.data;
    });

  return result;
}
