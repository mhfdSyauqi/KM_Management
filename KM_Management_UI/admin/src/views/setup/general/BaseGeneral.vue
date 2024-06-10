<script setup>
import IconExpand from '@/components/icons/IconExpand.vue'
import PrimaryButton from '@/components/buttons/PrimaryButton.vue'
import TextForm from '@/components/forms/TextForm.vue'
import NumberForm from '@/components/forms/NumberForm.vue'
import TextEditor from '@/components/editors/TextEditor.vue'

import { Disclosure, DisclosureButton, DisclosurePanel } from '@headlessui/vue'

import {
  generalModel,
  generalError,
  GetSetupGeneral,
  HandleCategoriesLimit,
  HandleTypingLimit,
  HandleIdleLimit,
  HandleOthersLimit,
  HandleEmptyMailing,
  HandleEmptyHelpdesk,
  HandleSave
} from '@/components/pages/setup/general/useGeneral.js'
import { onMounted } from 'vue'

onMounted(async () => {
  await GetSetupGeneral()
})
</script>

<template>
  <div class="flex items-center align-middle gap-3 mb-3 bg-tea">
    <p class="text-sm">Setup</p>

    <span> > </span>
    <p class="text-sm text-orange-400">General</p>
  </div>

  <div class="w-full rounded-lg bg-white min-h-[25%] py-5 px-8">
    <div class="flex gap-2.5 items-center justify-between">
      <h1 class="text-2xl font-bold text-green-800">General</h1>

      <PrimaryButton class="w-32" @click.prevent="HandleSave">Save Changes</PrimaryButton>
    </div>

    <br />

    <div class="flex flex-col gap-3">
      <!-- Categories -->
      <Disclosure v-slot="{ open }">
        <div class="border px-5 rounded-2xl select-none" :class="open && 'border-orange-400 pb-7'">
          <DisclosureButton class="py-2 flex justify-between w-full">
            <span class="font-medium">Top Categories</span>
            <IconExpand :class="open && 'rotate-90 transform'" />
          </DisclosureButton>
          <DisclosurePanel>
            <section class="flex gap-10">
              <NumberForm
                :name="'Layer 1'"
                required
                :min="3"
                :max="12"
                :step="1"
                :error="generalError.category.layer_one_limit"
                v-model="generalModel.category.layer_one_limit"
                @input="HandleCategoriesLimit"
              />
              <NumberForm
                :name="'Layer 2'"
                required
                :min="3"
                :max="12"
                :step="1"
                :error="generalError.category.layer_two_limit"
                v-model="generalModel.category.layer_two_limit"
                @input="HandleCategoriesLimit"
              />
              <NumberForm
                :name="'Layer 3'"
                required
                :min="3"
                :max="12"
                :step="1"
                :error="generalError.category.layer_three_limit"
                v-model="generalModel.category.layer_three_limit"
                @input="HandleCategoriesLimit"
              />
              <NumberForm
                :name="'Suggestion'"
                required
                :min="3"
                :max="12"
                :step="1"
                :error="generalError.category.suggestion_limit"
                v-model="generalModel.category.suggestion_limit"
                @input="HandleCategoriesLimit"
              />
            </section>
          </DisclosurePanel>
        </div>
      </Disclosure>
      <!-- Set Time -->
      <Disclosure v-slot="{ open }">
        <div class="border px-5 rounded-2xl select-none" :class="open && 'border-orange-400 pb-7'">
          <DisclosureButton class="py-2 flex justify-between w-full">
            <span class="font-medium">Set Time</span>
            <IconExpand :class="open && 'rotate-90 transform'" />
          </DisclosureButton>
          <DisclosurePanel>
            <section class="flex gap-10 w-1/2">
              <NumberForm
                :name="'Delay Typing (second)'"
                required
                :min="0.5"
                :max="1.5"
                :step="0.1"
                :error="generalError.timing.delay_typing"
                v-model="generalModel.timing.delay_typing"
                @input="HandleTypingLimit"
              />
              <NumberForm
                :name="'Idle (minute)'"
                required
                :min="5"
                :max="30"
                :step="5"
                :error="generalError.timing.idle_duration"
                v-model="generalModel.timing.idle_duration"
                @input="HandleIdleLimit"
              />
            </section>
          </DisclosurePanel>
        </div>
      </Disclosure>
      <!-- Email History -->
      <Disclosure v-slot="{ open }">
        <div class="border px-5 rounded-2xl select-none" :class="open && 'border-orange-400 pb-7'">
          <DisclosureButton class="py-2 flex justify-between w-full">
            <span class="font-medium">Email History</span>
            <IconExpand :class="open && 'rotate-90 transform'" />
          </DisclosureButton>
          <DisclosurePanel>
            <div class="bg-orange-100 p-3.5 text-sm xl:text-base rounded-tr-3xl">
              <p>
                Use <span class="font-medium text-orange-400">@virtualname </span> ➡️ value of
                Virtual Name in email subject
              </p>
            </div>

            <br />

            <div class="flex gap-10 w-full">
              <TextForm
                :name="'From'"
                :error="generalError.mailing.mail_history_from"
                v-model="generalModel.mailing.mail_history_from"
                required
                @input="HandleEmptyMailing"
              />
              <TextForm
                :name="'Subject'"
                :error="generalError.mailing.mail_history_subject"
                v-model="generalModel.mailing.mail_history_subject"
                required
                @input="HandleEmptyMailing"
              />
            </div>
          </DisclosurePanel>
        </div>
      </Disclosure>
      <!-- Helpdesk -->
      <Disclosure v-slot="{ open }">
        <div class="border px-5 rounded-2xl select-none" :class="open && 'border-orange-400 pb-7'">
          <DisclosureButton class="py-2 flex justify-between w-full">
            <span class="font-medium">Send Email To Helpdesk</span>
            <IconExpand :class="open && 'rotate-90 transform'" />
          </DisclosureButton>
          <DisclosurePanel>
            <div class="bg-orange-100 p-3.5 text-sm xl:text-base rounded-tr-3xl flex gap-20">
              <section>
                <p>
                  Use <span class="font-medium text-orange-400">@virtualname </span> ➡️ value of
                  Virtual Name in email
                </p>
                <p>
                  Use <span class="font-medium text-orange-400">@messageid </span> ➡️ value of
                  Message ID in email
                </p>
                <p>
                  Use <span class="font-medium text-orange-400">@description </span> ➡️ value of
                  Description in email
                </p>
                <p>
                  Use <span class="font-medium text-orange-400">@location </span> ➡️ value of
                  Location in email
                </p>
              </section>

              <section>
                <p>
                  Use <span class="font-medium text-orange-400">@username </span> ➡️ value of
                  Username in email
                </p>
                <p>
                  Use <span class="font-medium text-orange-400">@fullname </span> ➡️ value of Full
                  Name in email
                </p>
                <p>
                  Use <span class="font-medium text-orange-400">@jobtitle </span> ➡️ value of Job
                  Title in email
                </p>
              </section>
            </div>

            <br />

            <div class="flex flex-col gap-5 w-full">
              <TextForm
                :name="'From '"
                :error="generalError.helpdesk.mail_helpdesk_from"
                v-model="generalModel.helpdesk.mail_helpdesk_from"
                required
                @input="HandleEmptyHelpdesk"
              />
              <TextForm
                :name="'To '"
                :error="generalError.helpdesk.mail_helpdesk_to"
                v-model="generalModel.helpdesk.mail_helpdesk_to"
                required
                @input="HandleEmptyHelpdesk"
              />
              <TextForm
                :name="'Subject '"
                :error="generalError.helpdesk.mail_helpdesk_subject"
                v-model="generalModel.helpdesk.mail_helpdesk_subject"
                required
                @input="HandleEmptyHelpdesk"
              />

              <TextEditor
                v-model="generalModel.helpdesk.mail_helpdesk_content"
                v-model:html="generalModel.helpdesk.mail_helpdesk_content_html"
                :old-input="generalModel.helpdesk.mail_helpdesk_content_old"
                :error="generalError.helpdesk.mail_helpdesk_content"
              />
            </div>
          </DisclosurePanel>
        </div>
      </Disclosure>
      <!-- Others -->
      <Disclosure v-slot="{ open }">
        <div class="border px-5 rounded-2xl select-none" :class="open && 'border-orange-400 pb-7'">
          <DisclosureButton class="py-2 flex justify-between w-full">
            <span class="font-medium">Others</span>
            <IconExpand :class="open && 'rotate-90 transform'" />
          </DisclosureButton>
          <DisclosurePanel>
            <section class="flex gap-10 w-1/4">
              <NumberForm
                :name="'Keyword Length (minimum)'"
                required
                :min="3"
                :error="generalError.others.keywords"
                v-model="generalModel.others.keywords"
                @input="HandleOthersLimit"
              />
            </section>
          </DisclosurePanel>
        </div>
      </Disclosure>
    </div>
  </div>
</template>

<style scoped></style>