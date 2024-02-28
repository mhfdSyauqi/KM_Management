import Swal from 'sweetalert2'

const ConfirmSwal = Swal.mixin({
  customClass: {
    cancelButton:
      'font-semibold rounded-xl bg-white text-red-600 py-2 px-5 hover:bg-red-800 hover:text-white active:scale-95',
    confirmButton:
      'font-semibold rounded-xl border text-white bg-green-800 py-2 px-5 hover:bg-teal-700 active:scale-95 ml-2'
  },
  buttonsStyling: false,
  showConfirmButton: true,
  showCancelButton: true,
  confirmButtonText: 'Yes',
  cancelButtonText: 'No',
  reverseButtons: true,
  icon: 'info',
  title: 'Are you sure ?'
})

const ErrorSwal = Swal.mixin({
  showConfirmButton: false,
  icon: 'error',
  timer: 1700,
  title: 'Something went wrong'
})

const ToastSwal = Swal.mixin({
  toast: true,
  position: 'top-end',
  showConfirmButton: false,
  timer: 1700,
  timerProgressBar: true
})

export { ConfirmSwal, ErrorSwal, ToastSwal }