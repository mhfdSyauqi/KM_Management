import { PostAllSelectedTopIssueAsync} from '@/api/reqCategory.js'
import { ref } from 'vue'

const errorAllSelected = ref({
  name: { isError: false, message: '' },
})


async function HandlePublishAllSelected() {

  const postSelected = await PostAllSelectedTopIssueAsync()

  if (!postSelected.is_success) {
    if (postSelected.error.statuscode === 400) {
      errorAllSelected.value.name.isError = true
      errorAllSelected.value.name.message = postSelected.error.message
    }
    return postSelected.error.statuscode
  }
  return true
}



export { errorAllSelected, HandlePublishAllSelected}