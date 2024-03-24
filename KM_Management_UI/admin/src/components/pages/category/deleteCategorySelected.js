import { DeleteCategoryTopIssueSelectedAsync} from '@/api/reqCategory.js'
import { ref } from 'vue'

const deleteSelected = ref({
    Sequence:null,
})

const errorDeleted = ref({
  name: { isError: false, message: '' },
})


async function HandleDelete() {

  const patchCategory = await DeleteCategoryTopIssueSelectedAsync(
    deleteSelected.value.Sequence
  )

  if (!patchCategory.is_success) {
    if (patchCategory.error.statuscode === 400) {
      errorDeleted.value.name.isError = true
      errorDeleted.value.name.message = patchCategory.error.message
    }
    return patchCategory.error.statuscode
  }
  return true
}

export { deleteSelected, errorDeleted, HandleDelete }